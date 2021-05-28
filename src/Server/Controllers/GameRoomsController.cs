using System.Collections.Generic;
using System.Linq;
using Eumel.Core;
using Eumel.Server.Services;
using Eumel.Shared.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Eumel.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameRoomsController : ControllerBase
    {
        private readonly IGameRoomRepo _repo;

        public GameRoomsController(IGameRoomRepo gameRoomService)
        {
            _repo = gameRoomService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GameRoomData>> Get()
            => _repo
                .FindAll()
                .Select(ConvertRoomToDto)
                .ToList();

        [HttpGet("{name:length(24)}", Name = "GetRoom")]
        public ActionResult<GameRoomData> GetRoom(string name)
        {
            var room = _repo.FindByName(name);

            if (room == null)
            {
                return NotFound();
            }

            return ConvertRoomToDto(room);
        }

        private GameRoomData ConvertRoomToDto(EumelGameRoom room)
        {
            var def = room.Definition;
            return new GameRoomData
            {
                Name = def.Name,
                Status = (int)room.Status,
                Players = def.Players
                    .Select(p => new GamePlayerData { Name = p.Name, IsHuman = p.Type == PlayerType.Human })
                    .ToArray()
            };
        }

        [HttpPost]
        public ActionResult<GameRoomData> Create(GameRoomData roomData)
        {
            var name = roomData.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("invalid room name given");
            }
            if (_repo.ExistsWithName(name))
            {
                return BadRequest($"Room name '{name}' taken");
            }

            var players = roomData.Players
                .Select(p => new PlayerInfo(p.Name, p.IsHuman ? PlayerType.Human : PlayerType.Bot))
                .ToList();
            var room = new EumelGameRoomDefinition(name, players);
            _repo.Insert(room);

            return NoContent();
            // return CreatedAtRoute(nameof(GetRoom), new { name = room.Name }, room);
        }


        // [HttpPut("{id:length(24)}")]
        // public IActionResult Update(string id, Book bookIn)
        // {
        //     var book = _bookService.Get(id);

        //     if (book == null)
        //     {
        //         return NotFound();
        //     }

        //     _bookService.Update(id, bookIn);

        //     return NoContent();
        // }

        // [HttpDelete("{id:length(24)}")]
        // public IActionResult Delete(string id)
        // {
        //     var book = _bookService.Get(id);

        //     if (book == null)
        //     {
        //         return NotFound();
        //     }

        //     _bookService.Remove(book.Id);

        //     return NoContent();
        // }

    }
}