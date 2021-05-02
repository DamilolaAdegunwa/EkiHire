﻿using EkiHire.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using EkiHire.Core.Domain.DataTransferObjects;
using EkiHire.Core.Domain.Entities;

namespace EkiHire.Core.Domain.DataTransferObjects
{
    public class KeywordDTO : FullAuditedEntity
    {
        #region keywords
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public Subcategory Subcategory { get; set; }
        #endregion

        public static implicit operator KeywordDTO(Keyword model)
        {
            try
            {
                if (model != null)
                {
                    KeywordDTO response = new KeywordDTO
                    {
                        Name = model.Name,
                        Subcategory = model.Subcategory
                    };
                    return response;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}