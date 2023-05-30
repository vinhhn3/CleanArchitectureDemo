using CleanArchitectureDemo.Application.Interfaces.Repositories;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Persistence.Repositories;

using Moq;

namespace CleanArchitectureDemo.UnitTest
{
    internal class GenericRepositoryTest
    {
        private PlayerRepository _playerRepository;
        private Mock<IGenericRepository<Player>> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IGenericRepository<Player>>();
            _playerRepository = new PlayerRepository(_mockRepository.Object);
        }

        [Test]
        public async Task GetPlayersByClubAsync_WithValidClubId_ReturnsPlayers()
        {
            // Arrange
            int clubId = 1;
            List<Player> players = new List<Player>
        {
            new Player { ClubId = clubId },
            new Player { ClubId = clubId },
            new Player { ClubId = 2 } // Different club ID
        };

            // Set up the mock repository to return the players when queried
            _mockRepository.Setup(r => r.Entities)
                .Returns(players.AsQueryable());

            // Act
            List<Player> result = await _playerRepository.GetPlayersByClubAsync(clubId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(p => p.ClubId == clubId));
        }

        [Test]
        public async Task GetPlayersByClubAsync_WithInvalidClubId_ReturnsEmptyList()
        {
            // Arrange
            int clubId = 1;
            List<Player> players = new List<Player>
        {
            new Player { ClubId = 2 }, // Different club ID
            new Player { ClubId = 3 } // Different club ID
        };

            // Set up the mock repository to return the players when queried
            _mockRepository.Setup(r => r.Entities)
                .Returns(players.AsQueryable());

            // Act
            List<Player> result = await _playerRepository.GetPlayersByClubAsync(clubId);

            // Assert
            Assert.IsEmpty(result);
        }
    }
}
