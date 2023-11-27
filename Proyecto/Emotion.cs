using Google.Cloud.Firestore;
using System.Security.Cryptography;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Collections.Generic;

namespace Borrador3Proyecto
{
    [FirestoreData]
    public class Emotion
    {
        [FirestoreDocumentId]
        private string _id { get; set; }

        [FirestoreProperty]
        private string _Emocion { get; set; }

        [FirestoreProperty]
        public List<MusicInfo> MusicList { get; set; } = new List<MusicInfo>();

        public string Id { get { return _id; } set { _id = value; } }

        public string Emocion { get { return _Emocion; } set { _Emocion = value; } }

        public string Autor { get; set; }

        public Emotion(string id, string emocion)
        {
            Id = id;
            Emocion = emocion;
        }
    }
}