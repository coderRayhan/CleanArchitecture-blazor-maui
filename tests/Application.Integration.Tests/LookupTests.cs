using Application.Features.Lookups.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Integration.Tests
{
    public class LookupTests : BaseIntegrationTest
    {
        public LookupTests(IntegrationTestWebAppFactory factory) 
            : base(factory)
        {
        }

        [Fact]
        public async Task Create_Should_Add_NewLookupToDatabase()
        {
            // Arrange
            var command = new CreateLookupCommand("Division", "Division BN", "DIV", "Description", true);

            // Act
            await Sender.Send(command);

            // Assert
        }
    }
}
