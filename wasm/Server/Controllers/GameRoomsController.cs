using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorSignalRApp.Server.Services;
using BlazorSignalRApp.Shared.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Models;

namespace BlazorSignalRApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameRoomsController : ControllerBase
    {
        private readonly ILogger<GameRoomsController> _logger;
        private readonly IGameRoomService _roomService;

        public GameRoomsController(ILogger<GameRoomsController> logger, IGameRoomService gameRoomService)
        {
            _logger = logger;
            _roomService = gameRoomService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GameRoomData>> Get() =>
            _roomService.FindAll().Select(ConvertRoomToDto).ToList();

        [HttpGet("{id:length(24)}", Name = "GetRoom")]
        public ActionResult<GameRoomData> GetRoom(string id)
        {
            var room = _roomService.Find(id);

            if (room == null)
            {
                return NotFound();
            }

            return ConvertRoomToDto(room);
        }

        private GameRoomData ConvertRoomToDto(GameRoom room) =>
            new GameRoomData
            {
                Id = room.Id,
                Players = room.Players
                .Select(p => new GamePlayerData { Name = p.Name, IsHuman = p.IsHuman })
                .ToArray()
            };

        [HttpPost]
        public ActionResult<GameRoomData> Create(GameRoomData room)
        {
            if (_roomService.Find(room.Id) != null)
            {
                return BadRequest("Room id taken");
            }
            _roomService.Create(room);

            return NoContent();
            // return CreatedAtRoute(nameof(GetRoom), new { id = room.Id.ToString() }, room);
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