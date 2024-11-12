using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Infra.Orm.Compartilhado;

namespace NoteKeeper.WebApi.Config;

public static class DbContextExtensions
{
    public static bool AutoMigrateDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IContextoPersistencia>();

        bool migracacoConcluida = false;

        if (dbContext is NoteKeeperDbContext noteKeeperDbContext)
        {
            migracacoConcluida = MigradorBancoDados.AtualizarBancoDados(noteKeeperDbContext);
        }

        return migracacoConcluida;
    }
}
