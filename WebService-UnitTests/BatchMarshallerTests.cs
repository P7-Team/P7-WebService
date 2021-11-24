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

            Batch batch = BatchMarshaller.MarshalBatch(new Dictionary<string, string>(), new List<(string, Stream)>(), user);

            Assert.Equal(user.Username, batch.OwnerUsername);
        }

        [Fact]
        public void MarshallBatch_GivenSourceFileInformation_ReturnsBatchWithGivenSource()
        {
            Dictionary<string, string> formdata = new Dictionary<string, string>();
            formdata.Add("executableEncoding", "unicode");
            formdata.Add("executableLanguage", "Erlang");
            formdata.Add("executableExtension", ".erl");
            List<(string, Stream)> files = new List<(string, Stream)>();
            Stream sourceFile = new MemoryStream();
            files.Add(("executable", sourceFile));

            Batch batch = BatchMarshaller.MarshalBatch(formdata, files, CreateTestUser());
            
            Assert.Equal("unicode", batch.SourceFile.Encoding);
            Assert.Equal("Erlang", batch.SourceFile.Language);
            Assert.Equal(sourceFile, batch.SourceFile.Data);
        }

        [Fact]
        public void MarshallBatch_GivenReplicationFactorFormData_ReturnsBatchWithReplicationFactor()
        {
            Dictionary<string, string> formdata = new Dictionary<string, string>();
            formdata.Add("replicationfactor", "3");
            List<(string, Stream)> files = new List<(string, Stream)>();

            Batch batch = BatchMarshaller.MarshalBatch(formdata, files, CreateTestUser());

            Assert.Equal(3, batch.ReplicationFactor);
        }
    }
}
