using Backend.DataAccessObjects.PdfDAO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.PDFController;
[ApiController]
[Route("[controller]")]
public class PDFController : ControllerBase
{
    private IPDFInterface _pdfInterface;

    public PDFController(IPDFInterface pdfInterface)
    {
        _pdfInterface = pdfInterface;
    }
    
    
    [EnableCors] 
    [HttpPost]
    public async Task<ActionResult<string>> AddPDFAsync(string url, int adminId)
    {
        try
        {
            return StatusCode(200,await _pdfInterface.AddPDFAsync(url,adminId)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
      
      
    }
    
    [EnableCors]
    [HttpDelete]
    public async Task<ActionResult<string>> DeletePDFAsync(string url)
    {
        try
        {
            return StatusCode(200,await _pdfInterface.DeletePDFAsync(url)); 
        }
        catch (Exception e)
        {
            return   StatusCode(500, e.Message);
        }
    }
    
    
}