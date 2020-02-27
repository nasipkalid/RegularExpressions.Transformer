﻿using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace Platform.RegularExpressions.Transformer.Tests
{
    public class TransformersTests
    {
        [Fact]
        public void DebugOutputTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var secondStepReferenceText = "cccc";

            var transformer = new Transformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("b"), "c")
            });

            var steps = transformer.GetSteps(sourceText);

            Assert.Equal(2, steps.Count);
            Assert.Equal(firstStepReferenceText, steps[0]);
            Assert.Equal(secondStepReferenceText, steps[1]);
        }

        [Fact]
        public void DebugFilesOutputTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var secondStepReferenceText = "cccc";

            var sourceFilename = Path.GetTempFileName();
            File.WriteAllText(sourceFilename, sourceText, Encoding.UTF8);

            var transformer = new Transformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("b"), "c")
            });

            var targetFilename = Path.GetTempFileName();

            transformer.WriteStepsToFiles(sourceFilename, targetFilename, ".txt", skipFilesWithNoChanges: false);

            var firstStepReferenceFilename = $"{targetFilename}.0.txt";
            var secondStepReferenceFilename = $"{targetFilename}.1.txt";

            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.True(File.Exists(secondStepReferenceFilename));

            Assert.Equal(firstStepReferenceText, File.ReadAllText(firstStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(secondStepReferenceText, File.ReadAllText(secondStepReferenceFilename, Encoding.UTF8));

            File.Delete(sourceFilename);
            File.Delete(firstStepReferenceFilename);
            File.Delete(secondStepReferenceFilename);
        }

        [Fact]
        public void FilesWithNoChangesSkipedTest()
        {
            var sourceText = "aaaa";
            var firstStepReferenceText = "bbbb";
            var thirdStepReferenceText = "cccc";

            var sourceFilename = Path.GetTempFileName();
            File.WriteAllText(sourceFilename, sourceText, Encoding.UTF8);

            var transformer = new Transformer(new SubstitutionRule[] {
                (new Regex("a"), "b"),
                (new Regex("x"), "y"),
                (new Regex("b"), "c")
            });

            var targetFilename = Path.GetTempFileName();

            transformer.WriteStepsToFiles(sourceFilename, targetFilename, ".txt", skipFilesWithNoChanges: true);

            var firstStepReferenceFilename = $"{targetFilename}.0.txt";
            var secondStepReferenceFilename = $"{targetFilename}.1.txt";
            var thirdStepReferenceFilename = $"{targetFilename}.2.txt";

            Assert.True(File.Exists(firstStepReferenceFilename));
            Assert.False(File.Exists(secondStepReferenceFilename));
            Assert.True(File.Exists(thirdStepReferenceFilename));

            Assert.Equal(firstStepReferenceText, File.ReadAllText(firstStepReferenceFilename, Encoding.UTF8));
            Assert.Equal(thirdStepReferenceText, File.ReadAllText(thirdStepReferenceFilename, Encoding.UTF8));

            File.Delete(sourceFilename);
            File.Delete(firstStepReferenceFilename);
            File.Delete(secondStepReferenceFilename);
            File.Delete(thirdStepReferenceFilename);
        }
    }
}
