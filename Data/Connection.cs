using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using static Google.Rpc.Context.AttributeContext.Types;

namespace Borrador3Proyecto
{
    public class Connection
    {
        public FirestoreDb FirestoreDb { get; set; }
        public string Autor { get; set; }
        public string SelectedEmotion { get; set; }

        public Connection(string selectedEmotion)
        {
            SelectedEmotion = selectedEmotion;
            Autor = "Rage Against the Machine";
            var filePath = @"Data\musicemotions-cf347-firebase-adminsdk-ld0qe-2a4f43257e.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filePath);
            FirestoreDb = FirestoreDb.Create("musicemotions-cf347");
        }

        public void InsertEmotionAndAuthor()
        {
            Emotion emotion = new Emotion(string.Empty, SelectedEmotion);

            var fbModel = MapEntityToFirestoreModel(emotion);
            var colRef = FirestoreDb.Collection("Emotion");
            var doc = colRef.AddAsync(fbModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public void InsertEmotionAndSongInfo(MusicInfo songInfo)
        {
            Emotion emotion = new Emotion(string.Empty, SelectedEmotion);

            var fbModel = MapEntityToFirestoreModel(emotion);
            var colRef = FirestoreDb.Collection("Emotion");
            var doc = colRef.AddAsync(fbModel).ConfigureAwait(false).GetAwaiter().GetResult();

            string emotionId = doc.Id;

            var emotionDocument = colRef.Document(emotionId);
            var emotionData = emotionDocument.GetSnapshotAsync().ConfigureAwait(false).GetAwaiter().GetResult().ConvertTo<Data.FirestoreModels.EmotionFirestoreModel>();

            if (emotionData.MusicList == null)
            {
                emotionData.MusicList = new List<Data.FirestoreModels.MusicInfoFirestoreModel>();
            }

            emotionData.MusicList.Add(new Data.FirestoreModels.MusicInfoFirestoreModel
            {
                Álbum = songInfo.Álbum,
                Autor = songInfo.Autor,
                Género = songInfo.Género,
                URL_de_la_Imagen_del_Álbum = songInfo.URL_de_la_Imagen_del_Álbum,
                URL_de_la_Imagen_del_Artista = songInfo.URL_de_la_Imagen_del_Artista,
                URL_del_Video = songInfo.URL_del_Video
            });

            emotionDocument.SetAsync(emotionData, SetOptions.MergeAll).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private Data.FirestoreModels.EmotionFirestoreModel MapEntityToFirestoreModel(Emotion emotion)
        {
            return new Data.FirestoreModels.EmotionFirestoreModel
            {
                Id = emotion.Id,
                Emocion = emotion.Emocion,
            };
        }
    }
}
