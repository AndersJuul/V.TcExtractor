﻿using CsvHelper.Configuration;
using V.TcExtractor.Domain.Model;

namespace V.TcExtractor.Infrastructure.CsvStorage.Mappers;

public class Match1Map : ClassMap<Match1>
{
    public Match1Map()
    {
        Map(m => m.ModuleRequirementId).Name("ModuleRequirementId").Index(0);
        Map(m => m.TestCases).Name("TestCases").Index(1);
    }
}