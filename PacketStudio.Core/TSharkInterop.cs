using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CliWrap.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PacketStudio.DataAccess;

namespace PacketStudio.Core
{
	public class TSharkInterop
	{
		private string _tsharkPath;
		private TempPacketsSaver _packetsSaver;

		public string TsharkPath
		{
			get => _tsharkPath;
			set
			{
				if (!File.Exists(value))
				{
					throw new FileNotFoundException("Couldn't find TShark at the given location. Path: " + value);
				}
				_tsharkPath = value;
			}
		}

		public TSharkInterop(string tsharkPath)
		{
			TsharkPath = tsharkPath; // This also checks if the file exists
			_packetsSaver = new TempPacketsSaver();
		}

		public async Task<XElement> GetPdmlAsync(byte[] packetBytes)
		{
			return await GetPdmlAsync(packetBytes, CancellationToken.None);
		}

		public async Task<XElement> GetPdmlAsync(IEnumerable<byte[]> packets, int packetIndex)
		{
			return await GetPdmlAsync(packets, packetIndex, CancellationToken.None);
		}


		public async Task<XElement> GetPdmlAsync(byte[] packetBytes, CancellationToken token)
		{
			return await GetPdmlAsync(new List<byte[]>() { packetBytes }, 0, token);
		}

		public Task<XElement> GetPdmlAsync(IEnumerable<byte[]> packets, int packetIndex, CancellationToken token)
		{
			return Task.Run((() =>
			{
				string pcapPath = _packetsSaver.WritePackets(packets);
				token.ThrowIfCancellationRequested();

				CliWrap.Cli cli = new CliWrap.Cli(_tsharkPath);
				Task<ExecutionOutput> tsharkTask = cli.ExecuteAsync($"-r {pcapPath} -2 -T pdml", token);
				tsharkTask.Wait();

				if (tsharkTask.IsCanceled) // task was canceled
					throw new TaskCanceledException();

				ExecutionOutput res = tsharkTask.Result;
				if (res.ExitCode != 0) // TShark returned an error exit code
				{
					// If we are cancelled, we don't actually care about the exit code
					token.ThrowIfCancellationRequested();

					// Show the exit code + errors
					throw new Exception($"TShark returned with exit code: {res.ExitCode}\r\n{tsharkTask.Result.StandardError}");
				}

				string xml = res.StandardOutput;

				XElement element = ParsePdmlAsync(xml, packetIndex, token).Result;
				xml = null; //Hoping GC will get the que
				token.ThrowIfCancellationRequested();

				return element;
			}));
		}

		public async Task<JObject> GetJsonRawAsync(byte[] packetBytes)
		{
			return await GetJsonRawAsync(packetBytes, CancellationToken.None);
		}

		public async Task<JObject> GetJsonRawAsync(IEnumerable<byte[]> packets, int packetIndex)
		{
			return await GetJsonRawAsync(packets, packetIndex, CancellationToken.None);
		}


		public async Task<JObject> GetJsonRawAsync(byte[] packetBytes, CancellationToken token)
		{
			return await GetJsonRawAsync(new List<byte[]>() { packetBytes }, 0, token);
		}

		public Task<JObject> GetJsonRawAsync(IEnumerable<byte[]> packets, int packetIndex, CancellationToken token)
		{
			return Task.Run((() =>
			{
				string pcapPath = _packetsSaver.WritePackets(packets);
				token.ThrowIfCancellationRequested();

				CliWrap.Cli cli = new CliWrap.Cli(_tsharkPath);
				Task<ExecutionOutput> tsharkTask = cli.ExecuteAsync($"-r {pcapPath} -2 -T jsonraw --no-duplicate-keys", token);
				tsharkTask.Wait();

				if (tsharkTask.IsCanceled) // task was canceled
					throw new TaskCanceledException();

				ExecutionOutput res = tsharkTask.Result;
				if (res.ExitCode != 0) // TShark returned an error exit code
				{
					// If we are cancelled, we don't actually care about the exit code
					token.ThrowIfCancellationRequested();

					// Show the exit code + errors
					throw new Exception($"TShark returned with exit code: {res.ExitCode}\r\n{tsharkTask.Result.StandardError}");
				}

				string jsonArray = res.StandardOutput;
				
				JObject jsonPacket = ParseJsonAsync(jsonArray, packetIndex, token).Result;
				
				jsonArray = null; //Hoping GC will get the que
				token.ThrowIfCancellationRequested();

				return jsonPacket;
			}));
		}

		private Task<XElement> ParsePdmlAsync(string xml, int packetIndex, CancellationToken token)
		{
			return Task.Run((() =>
			{
				token.ThrowIfCancellationRequested();

				// This is most likely the heavy operation in this method. So checking the token right before and after
				XDocument xdoc = XDocument.Parse(xml);

				token.ThrowIfCancellationRequested();

				// Get the right packet according to the given index (parameter)
				var skippedHeaderNode =
					((XContainer)xdoc.FirstNode.NextNode.NextNode); // First few tags are declerations present in any pdml, skipping
				XElement packetElement =
					skippedHeaderNode.Elements("packet").ElementAt(packetIndex); // Getting the x-th 'packet' node
				
				var singlePacketXml = packetElement.ToString();
				XElement elem = XElement.Parse(singlePacketXml);
				
				// Hoping GC will get the cue
				singlePacketXml = null;
				packetElement = null;
				xdoc = null;


				return elem;
			}), token);
		}
		private Task<JObject> ParseJsonAsync(string json, int packetIndex, CancellationToken token)
		{
			return Task.Run((() =>
			{
				token.ThrowIfCancellationRequested();

				// This is most likely the heavy operation in this method. So checking the token right before and after
				var serializer = JsonSerializer.CreateDefault();
				JsonReader reader = new JsonTextReader(new StringReader(json));
				var deserialized = serializer.Deserialize(reader);
				JArray asArray = deserialized as JArray;
				

				token.ThrowIfCancellationRequested();

				// Get the right packet according to the given index (parameter)
				JObject packet = asArray.ElementAt(packetIndex) as JObject;

				// Hoping GC will get the cue
				reader.Close();
				reader = null;
				asArray = null;


				return packet;
			}), token);
		}

		public Task<TSharkCombinedResults> GetPdmlAndJsonAsync(IEnumerable<byte[]> packets, int packetIndex,
			CancellationToken token)
		{
			Task<XElement> pdmlTask = GetPdmlAsync(packets, packetIndex, token);
			Task<JObject> jsonTask = GetJsonRawAsync(packets, packetIndex, token);

			return Task.Factory.StartNew(() =>
			{
				XElement pdml = pdmlTask.Result;
				JObject json = jsonTask.Result;
				return new TSharkCombinedResults(pdml, json);
			},token);
		}
	}
}
