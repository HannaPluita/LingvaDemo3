﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lingva.WebAPI.Dto
{
    public class DictionaryRecordCreatingDTO
    {
        public int User { get; set; }
        public string Word { get; set; }
        public string Translation { get; set; }
        public string Language { get; set; }
        public string Context { get; set; }
        public string Picture { get; set; }
    }
}
