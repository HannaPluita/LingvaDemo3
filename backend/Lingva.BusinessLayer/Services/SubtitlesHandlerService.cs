using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lingva.BusinessLayer.Contracts;
using Lingva.BusinessLayer.DTO;
using Lingva.BusinessLayer.SubtitlesParser.Classes;
using Lingva.DataAccessLayer.Entities;
using Lingva.DataAccessLayer.Repositories;

namespace Lingva.BusinessLayer.Services
{
    public class SubtitlesHandlerService : ISubtitlesHandlerService
    {
        private readonly IUnitOfWorkParser _unitOfWork;

        public SubtitlesHandlerService(IUnitOfWorkParser unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddSubtitles(SubtitlesRowDTO[] subDTO, string path, int? filmId)
        {
            _unitOfWork.Subtitles.Create(new Subtitle()
            {
                Path = path,
                FilmId = filmId,
                //LanguageName = "en"
            });

            //int subId = _unitOfWork.Subtitles.Get(p => p.Path == path).Id;
            //subDTO.ToList().ForEach(n => n.SubtitlesId = subId);

            foreach(var sub in subDTO)
            {
                _unitOfWork.SubtitleRows.Create(new SubtitleRow()
                {
                    Id = sub.LineNumber,
                    Value = sub.Value,
                    //SubtitleId = subId,
                    StartTime = sub.StartTime,
                    EndTime = sub.EndTime
                });
            }

            _unitOfWork.Save();
        }

        public SubtitlesRowDTO[] Parse(Stream subtitles)
        {
            var encoding = DetectEncoding(subtitles);

            var parser = new SubtitlesParser.Classes.Parsers.SrtParser();
            List<SubtitleItem> items = parser.ParseStream(subtitles, encoding);

            SubtitlesRowDTO[] subDTO = items.Select((n, index) => new SubtitlesRowDTO()
            {
                LineNumber = index + 1,
                Value = String.Join(" ", n.Lines),
                StartTime = new TimeSpan(0, 0, 0, 0, n.StartTime),
                EndTime = new TimeSpan(0, 0, 0, 0, n.EndTime),
            }).ToArray();

            return subDTO;
        }

        private Encoding DetectEncoding(Stream stream)
        {
            Ude.CharsetDetector cdet = new Ude.CharsetDetector();
            cdet.Feed(stream);
            cdet.DataEnd();
            if (cdet.Charset != null)
            {
                return Encoding.GetEncoding(cdet.Charset);
            }
            else
            {
                throw new FormatException("Encoding unrecognized");
            }
        }
    }
}
