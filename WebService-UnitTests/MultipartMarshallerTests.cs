using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class MultipartMarshallerTests
    {
        private class SectionedStringReader : ISectionedDataReader<string>
        {
            private Stack<string> _sections;

            public SectionedStringReader(List<string> sections)
            {
                _sections = new Stack<string>(sections);
            }

            public string GetContentDisposition(string section)
            {
                if(section.Contains("withFile"))
                    return "Content-Disposition: form-data; name=\"myFile\"; filename=\"foo.txt\"";
                else 
                    return "Content-Disposition: form-data; name=\"description\"";
            }

            public async Task<FileStream> ReadFileSectionAsync(string section)
            {
                return await Task.FromResult(new FileStream(section, FileMode.Append));
            }

            public async Task<KeyValuePair<string, string>> ReadFormDataSectionAsync(string section)
            {
                return await Task.FromResult(new KeyValuePair<string, string>(section, "value"));
            }

            public async Task<string> ReadSection()
            {
                if (_sections.Count == 0)
                    return null;
                return await Task.FromResult(_sections.Pop());
            }
        }

        [Fact]
        public void MultipartMarshaller_ReaderWithNoSections_EmptyFileAndFormData()
        {
            MultipartMarshaller<string> marshaller = new MultipartMarshaller<string>(new SectionedStringReader(new List<string>()));

            Assert.Empty(marshaller.GetFormData());
            Assert.Empty(marshaller.GetFileStreams());
        }

        [Fact]
        public void MultipartMarshaller_ReaderWithFileData_SingleFile()
        {
            List<string> sections = new List<string>
            {
                "withFile"
            };
            MultipartMarshaller<string> marshaller = new MultipartMarshaller<string>(new SectionedStringReader(sections));

            Assert.Empty(marshaller.GetFormData());
            Assert.Contains("withFile", marshaller.GetFileStreams().First().Name);
        }

        [Fact]
        public void MultipartMarshaller_ReaderWithFormData_SingleFormEntry()
        {
            List<string> sections = new List<string>
            {
                "test"
            };
            MultipartMarshaller<string> marshaller = new MultipartMarshaller<string>(new SectionedStringReader(sections));

            Assert.Empty(marshaller.GetFileStreams());
            Assert.Contains("test", marshaller.GetFormData().Keys);
        }

        [Fact]
        public void MultipartMarshaller_ReaderWithFileAndFormData_NonEmptyFileAndFormData()
        {
            List<string> sections = new List<string>
            {
                "test",
                "withFileTest"
            };
            MultipartMarshaller<string> marshaller = new MultipartMarshaller<string>(new SectionedStringReader(sections));

            Assert.Contains("test", marshaller.GetFormData().Keys);
            Assert.Contains("withFile", marshaller.GetFileStreams().First().Name);
        }
    }
}
