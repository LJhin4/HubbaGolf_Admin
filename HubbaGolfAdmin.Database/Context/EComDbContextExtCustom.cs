using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HubbaGolfAdmin.Database
{
    public class EComDbContextExtCustom : EComDbContext
    {
        private readonly SessionStore _SessionStore;
        public EComDbContextExtCustom(DbContextOptions<EComDbContext> options, SessionStore sessionStore) : base(options)
        {
            _SessionStore = sessionStore;
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var zUserId = "System or not login";
            var zUserName = "System or not login";

            if (_SessionStore != null)
            {
                var zUserDto = _SessionStore.UserLogged;
                if (zUserDto != null)
                {
                    zUserId = zUserDto.Id.ToString();
                    zUserName = zUserDto.UserName;
                }
            }

            UpdateSoftDeleteStatuses(zUserId, zUserName);
            return base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateSoftDeleteStatuses(string userId, string userName)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (HasProperty(entry.Entity, "RecordStatus"))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["RecordStatus"] = 1;
                            entry.CurrentValues["CreatedOn"] = DateTime.Now;
                            entry.CurrentValues["CreatedBy"] = userId;
                            entry.CurrentValues["CreatedName"] = userName;
                            entry.CurrentValues["ModifiedOn"] = DateTime.Now;
                            entry.CurrentValues["ModifiedBy"] = userId;
                            entry.CurrentValues["ModifiedName"] = userName;
                            break;

                        case EntityState.Modified:
                            entry.CurrentValues["RecordStatus"] = 2;
                            entry.CurrentValues["ModifiedOn"] = DateTime.Now;
                            entry.CurrentValues["ModifiedBy"] = userId;
                            entry.CurrentValues["ModifiedName"] = userName;
                            break;

                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["RecordStatus"] = 99;
                            entry.CurrentValues["ModifiedOn"] = DateTime.Now;
                            entry.CurrentValues["ModifiedBy"] = userId;
                            entry.CurrentValues["ModifiedName"] = userName;
                            break;
                    }
                }

                if (HasProperty(entry.Entity, "Author"))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["Author"] = userName;
                            break;
                    }
                }
            }
        }
        private static bool HasProperty(object entry, string propertyName)
        {
            var type = entry.GetType();
            var property = type.GetProperty(propertyName);
            return property != null;
        }
        public async Task<int> DeleteRecordsWithStatus99()
        {
            int totalDeleted = 0;

            var entityTypes = Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var entityTypeClrType = entityType.ClrType;
                var recordStatusProperty = entityTypeClrType.GetProperty("RecordStatus");

                if (recordStatusProperty != null)
                {
                    var entitiesToDelete = ChangeTracker.Entries()
                        .Where(e => e.State != EntityState.Detached &&
                                    e.Metadata.ClrType == entityTypeClrType &&
                                    (int?)recordStatusProperty.GetValue(e.Entity) == 99)
                        .Select(e => e.Entity)
                        .ToList();

                    foreach (var entity in entitiesToDelete)
                    {
                        Entry(entity).State = EntityState.Deleted;
                    }

                    totalDeleted += entitiesToDelete.Count;
                }
            }

            await SaveChangesAsync();

            return totalDeleted;
        }
    }
}
