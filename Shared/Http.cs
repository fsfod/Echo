//
//  PT SYNC
//  2013 Lee Brunjes
//
//  A one way file sync System. 
//
//
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Json;
using System.IO;
using Shared;

namespace Shared
{
	public static class Http
	{
		public static Dictionary<string,SyncItem> GetHashList(){
			Dictionary<string,SyncItem> Hashlist = new Dictionary<string,SyncItem> ();

			//get remote hash data
			string HashJSON = getHashData ();

			//read JSON
			JsonTextParser parser = new JsonTextParser ();
			JsonObject hashes = parser.Parse(HashJSON);

			foreach (JsonObject field in hashes as JsonObjectCollection) {
		
				if(field.GetValue().GetType().Name !="String" ){

					List<JsonObject> obj  = (List<JsonObject>)field.GetValue();



					SyncFile sf =new SyncFile(field.Name.ToString(), obj);		                                       
					Hashlist.Add (field.Name, sf);
				}
				else{
					//Console.WriteLine(field.Name, field.GetValue().ToString());
				}
			}

			return Hashlist;
		}

		private static string getHashData(){
			string data = "";

				HttpWebRequest req =  (HttpWebRequest)WebRequest.Create(Settings.HashServer);
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
				HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
				StreamReader read = new StreamReader( resp.GetResponseStream());
				data = read.ReadToEnd();
				//Console.WriteLine(data);
				resp.Close();
			
			return data;
		}
/*
		public static int DownloadFile (string fileName){
			int filesChanged = 0;
			string dir = "";
			//remove file if it exists.
			if (File.Exists (Settings.LocalDirectory + fileName)) {
				File.Delete (Settings.LocalDirectory + fileName);
			}

				//make sure the requred directoreis exist
				dir = Path.GetDirectoryName(Settings.LocalDirectory + fileName);
				if(!Directory.Exists(dir)){
					Console.WriteLine("Createing dir");
					Directory.CreateDirectory(dir);
				}

				using (FileStream file  = File.OpenWrite(Settings.LocalDirectory+fileName)) {


					HttpWebRequest http = (HttpWebRequest)WebRequest.Create (Settings.HTTPServer + fileName.Replace('\\','/'));
					

					http.Credentials = new NetworkCredential (Settings.RemoteUser, Settings.RemotePassword);
					HttpWebResponse response = (HttpWebResponse)http.GetResponse ();

					Stream ftpdata = response.GetResponseStream();
					ftpdata.CopyTo (file);
					filesChanged++;
				}


			return filesChanged;

		}*/


	}
}

