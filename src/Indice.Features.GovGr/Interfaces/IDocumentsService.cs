﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Indice.Features.GovGr.Models;
using Microsoft.Extensions.Localization;
using Polly.Utilities;

namespace Indice.Features.GovGr.Interfaces
{
    /// <summary>
    /// GovGr doucments service definition
    /// </summary>
    public interface IDocumentsService
    {
        /// <summary>
        /// Post to the documents service
        /// </summary>
        /// <param name="document">The request</param>
        /// <returns>Payload containing all doucment related data. May also contain binary data as base64 encoded property</returns>
        /// <exception cref="Types.GovGrServiceException"></exception>
        Task<DoucumentsReponse> PostAsync(DocumentData document);
    }
}
