using System;
using System.Collections.Generic;
using System.Text;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class MultipartHelperTests
    {
        [Fact]
        public void GetBoundary_ReturnsBoundary()
        {
            // Example header taken from https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Type
            string contentType = "Content-Type: multipart/form-data; boundary=---------------------------974767299852498929531610575";
            const string expectedBoundary = "---------------------------974767299852498929531610575";

            Assert.Equal(expectedBoundary, MultipartHelper.GetBoundary(contentType));
        }

        [Fact]
        public void IsFileData_ContentDispositionHeaderContainsFilename_ReturnsTrue()
        {
            string contentDisposition = "Content-Disposition: form-data; name=\"myFile\"; filename=\"foo.txt\"";
            Assert.True(MultipartHelper.IsFileData(contentDisposition));
        }

        [Fact]
        public void IsFileData_ContentDispositionHeaderDoesNotContainFileName_ReturnsFalse()
        {
            string contentDisposition = "Content-Disposition: form-data; name=\"description\"";
            Assert.False(MultipartHelper.IsFileData(contentDisposition));
        }
    }
}
