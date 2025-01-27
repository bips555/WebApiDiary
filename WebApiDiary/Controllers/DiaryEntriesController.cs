using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDiary.Data;
using WebApiDiary.Models;

namespace WebApiDiary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryEntriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DiaryEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
        {
            return await _context.DiaryEntries.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryEntry>> GetDiaryEntryById(int? id )
        {
            var diaryEntry =await _context.DiaryEntries.FindAsync(id);
            if(diaryEntry == null)
            {
                return NotFound();
            }
            return diaryEntry;
        }
        [HttpPost]
        public async Task<ActionResult<DiaryEntry>> PostDiaryEntry(DiaryEntry diaryEntry)
        {
            diaryEntry.Id = 0;
            _context.DiaryEntries.Add(diaryEntry);
            await _context.SaveChangesAsync();
            var resourceURI = Url.Action(nameof(GetDiaryEntryById), new { Id = diaryEntry.Id });
            return Created(resourceURI, diaryEntry);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiaryEntry(int id,[FromBody] DiaryEntry diaryEntry)
        {
            if(id != diaryEntry.Id)
            {
                return BadRequest();
            }
            _context.Entry(diaryEntry).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!DiaryEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiaryEntry(int id)
        {
           var entry = await _context.DiaryEntries.FindAsync(id) ;
           if(entry == null)
            {
                return NotFound();
            }
            _context.DiaryEntries.Remove(entry);
           await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool DiaryEntryExists(int id)
        {
            return _context.DiaryEntries.Any(e => e.Id == id);
        }
    }
}
