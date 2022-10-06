﻿using System;
using System.Collections.Generic;

namespace Indice.Features.Cases.Data.Models
{
    public class DbCaseType
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string DataSchema { get; set; }
        public string? Layout { get; set; }
        public string? Translations { get; set; }
        public string? LayoutTranslations { get; set; }
        public string? Tags { get; set; }
        public string? Config { get; set; }
        /// <summary>
        /// The allowed Roles that can create a new Case
        /// </summary>
        public string? CanCreateRoles { get; set; }
        /// <summary>
        /// Available checkpoints for this case type
        /// </summary>
        public virtual List<DbCheckpointType> CheckpointTypes { get; set; }
    }
}