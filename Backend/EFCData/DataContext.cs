using Microsoft.EntityFrameworkCore;
using Pgvector;
using Shared;

namespace Backend.EFCData;

public class DataContext : DbContext
{
     public DbSet<Admin> Admins { get; set; }
    public DbSet<PDF> PDFs { get; set; }
    public DbSet<Chunks> Chunks { get; set; }
    public DbSet<Chat_session> Chat_sessions { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Conversation_PDF_Chunks> ConversationPdfChunks{ get; set; }
    
    // protected readonly IConfiguration Configuration;
    // public DataContext(IConfiguration configuration,DbContextOptions<DataContext> options)
    // {
    //     Configuration = configuration;
    //     
    // }
    // protected override void OnConfiguring(DbContextOptionsBuilder options)
    // {
    //     options.UseNpgsql(Configuration.GetConnectionString("ChatBotDBCloud"), o => o.UseVector());
    // }
    
    private readonly IConfiguration _configuration;
    private readonly bool _useInMemoryDatabase;
    private readonly DbContextOptions<DataContext> _options;
    public DataContext(IConfiguration configuration, bool useInMemoryDatabase = false)
    {
        _configuration = configuration;
        _useInMemoryDatabase = useInMemoryDatabase;
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_useInMemoryDatabase)
        {
            optionsBuilder.UseInMemoryDatabase("Test_Database");
        }
        else
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ChatBotDBCloud"), o => o.UseVector());
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>()
            .HasKey(a => a.Id);
        
        modelBuilder.Entity<PDF>()
            .HasOne(p => p.Admin)
            .WithMany(a => a.PDFs)
            .HasForeignKey(p => p.AdminId);
////////////////////////////
        modelBuilder.Entity<Chunks>()
            .HasOne(pc => pc.PDF)
            .WithMany(p => p.PDFChuncks)
            .HasForeignKey(pc => pc.PDFId);

        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.Entity<Chunks>()
            .HasIndex(i => i.Embedding)
            .HasMethod("ivfflat")
            .HasOperators("vector_l2_ops");
      
 ////////////////////////////////       
        modelBuilder.Entity<Chat_session>()
            .HasKey(cs => cs.Id);
        
        modelBuilder.Entity<Conversation>()
            .HasOne(c => c.ChatSession)
            .WithMany(cs => cs.Conversations)
            .HasForeignKey(c => c.ChatSessionId);
        
        
        
        modelBuilder.Entity<Conversation_PDF_Chunks>()
            .HasKey(cp => new { cp.ConversationId, cp.PDFChuncksId });
          
        modelBuilder.Entity<Conversation_PDF_Chunks>()
            .HasOne(cp => cp.Conversation)
            .WithMany(c => c.ConversationPDFChunks)
            .HasForeignKey(cp => cp.ConversationId);

        modelBuilder.Entity<Conversation_PDF_Chunks>()
            .HasOne(cp => cp.PDF_Chuncks)
            .WithMany(pc => pc.PDFChunks)
            .HasForeignKey(cp => cp.PDFChuncksId);
        
        //////////////////////////////////////////////////////
        
        modelBuilder.Entity<Chunks>()
            .Property(c => c.Embedding)
            .HasConversion(
                v => ConvertEmbeddingToString(v)
                , v => ConvertStringToEmbedding(v)
            );
        ////////////////////////////////////////////////////////
            
    }

    private Vector? ConvertStringToEmbedding(object o)
    {
        throw new NotImplementedException();
    }

    private string ConvertEmbeddingToString(Vector vector)
    {
        return vector.ToString();
    }
}