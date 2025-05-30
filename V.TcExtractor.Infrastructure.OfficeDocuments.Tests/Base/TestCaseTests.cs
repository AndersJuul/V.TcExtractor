﻿using V.TcExtractor.Domain.Adapters;
using V.TcExtractor.Domain.Model;
using V.TcExtractor.Domain.Processors;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.CellAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.FileAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestCaseTableAdapters;
using V.TcExtractor.Infrastructure.OfficeDocuments.Adapters.TestResultTableAdapters;
using Xunit.Abstractions;

namespace V.TcExtractor.Infrastructure.OfficeDocuments.Tests.Base;

public abstract class TestCaseTests(ITestOutputHelper testOutputHelper)
{
    protected readonly ITestOutputHelper TestOutputHelper = testOutputHelper;
    protected readonly string TestDataPath = "C:\\Users\\aju\\Mit drev\\V";

    protected void Dump(IEnumerable<TestCase> testCases)
    {
        TestOutputHelper.WriteLine("TestCases :");
        TestOutputHelper.WriteLine($"---- {testCases.Count()} cases");
        foreach (var testCase in testCases)
        {
            TestOutputHelper.WriteLine($"Test : {testCase}");
        }

        TestOutputHelper.WriteLine("----");
    }

    protected void Dump(IEnumerable<ModuleRequirement> moduleRequirements)
    {
        TestOutputHelper.WriteLine("Module Requirements:");
        TestOutputHelper.WriteLine($"---- {moduleRequirements.Count()} module requirements");
        foreach (var moduleRequirement in moduleRequirements)
        {
            TestOutputHelper.WriteLine($"Module Requirement : {moduleRequirement}");
        }

        TestOutputHelper.WriteLine("----");
    }

    protected void Dump(DvplItem[] dvplItems)
    {
        TestOutputHelper.WriteLine("DVPL Items:");
        TestOutputHelper.WriteLine($"---- {dvplItems.Length} DVPL Items");
        foreach (var dvplItem in dvplItems)
        {
            TestOutputHelper.WriteLine($"DVPL Item : {dvplItem}");
        }

        TestOutputHelper.WriteLine("----");
    }

    protected static TestCaseFileProcessor GetWordFileProcessor()
    {
        var cellAdapter = new CellAdapter();
        var wordFileProcessor = new TestCaseFileProcessor(
        [
            new TestCaseTableAdapterId(cellAdapter),
            new TestCaseTableAdapterTestCaseIdAndDescriptionHeadersInRowZero(cellAdapter),
            new TestCaseTableAdapterTestCaseIdSubjectHeadersInColZero(cellAdapter),
            new TestCaseTableAdapterTestCaseInformationHeadersInRowZero(cellAdapter),
            new TestCaseTableAdapterTestCaseIdInitialConditionsHeadersInRowZero(cellAdapter)
        ], new DmsNumberAdapter());
        return wordFileProcessor;
    }

    protected static IModuleRequirementFileProcessor[] GetExcelFileProcessors()
    {
        return
        [
            new ModuleRequirementFileProcessorPsi(),
            new ModuleRequirementFileProcessorSpc()
        ];
    }

    protected TestResultNonScadaFileProcessor GetTestResultNonScadaFileProcessor()
    {
        var testResultFileProcessor =
            new TestResultNonScadaFileProcessor(
                [new TestResultTableAdapter(new CellAdapter(), new PassedTextAdapter())],
                new DmsNumberAdapter());
        return testResultFileProcessor;
    }

    protected void Dump(TestResult[] testResults)
    {
        TestOutputHelper.WriteLine("Test Results:");
        TestOutputHelper.WriteLine($"---- {testResults.Length} Test Results");
        foreach (var testResult in testResults)
        {
            TestOutputHelper.WriteLine($"Test Result : {testResult}");
        }

        TestOutputHelper.WriteLine("----");
    }

    protected void Dump(FileItem[] fileItems)
    {
        TestOutputHelper.WriteLine("File Items:");
        TestOutputHelper.WriteLine($"---- {fileItems.Length} File Items");
        foreach (var testResult in fileItems)
        {
            TestOutputHelper.WriteLine($"File Item: {testResult}");
        }

        TestOutputHelper.WriteLine("----");
    }
}