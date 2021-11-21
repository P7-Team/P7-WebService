using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebService;
using WebService.Models;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class BatchMarshallerTests
    {
        private User CreateTestUser(string username = "testUser", string password = "testPassword")
        {
            return new User(username, password);
        }

        [Fact]
        public void MarhallBatch_GivenUserDetails_ReturnsBatchWithGivenUserAsOwner()
        {
            User user = CreateTestUser();

            Batch batch = BatchMarshaller.MarshalBatch(new Dictionary<string, string>(), new List<FileStream>(), user);

            Assert.Equal(user.Username, batch.OwnerUsername);
        }

        [Fact]
        public void MarshallBatch_GivenSourceFileInformation_ReturnsBatchWithGivenSource()
        {
            Dictionary<string, string> formdata = new Dictionary<string, string>();
            formdata.Add("executableEncoding", "unicode");
            formdata.Add("executableLanguage", "Erlang");
            FileStream sourceFile = new FileStream("executable", FileMode.Truncate);
            List<FileStream> files = new List<FileStream>();
            files.Add(sourceFile);

            Batch batch = BatchMarshaller.MarshalBatch(formdata, files, CreateTestUser());

            Assert.Equal(sourceFile.Name, batch.SourceFile.Filename);
            Assert.Equal("unicode", batch.SourceFile.Encoding);
            Assert.Equal("Erlang", batch.SourceFile.Language);
            Assert.Equal(sourceFile, batch.SourceFile.Data);
        }
    }
}
