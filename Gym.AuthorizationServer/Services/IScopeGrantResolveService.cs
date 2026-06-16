using Gym.AuthorizationServer.Infrastructure.Entities.Scopes;
using Gym.AuthorizationServer.Infrastructure.Entities.UserConsents;
using Gym.AuthorizationServer.Shared.Abstractions;

namespace Gym.AuthorizationServer.Services
{
    public interface IScopeGrantResolveService
    {
        Task<Result<ICollection<ScopeInfo>>> Resolve(String roleId, String protectedResouceId, String? requestedScopes, CancellationToken cancellationToken); 
    }

    public class ScopeGrantResolveService(IScopeRepository _scopeRepository, IScopeChecker _scopeChecker) : IScopeGrantResolveService
    {
        public async Task<Result<ICollection<ScopeInfo>>> Resolve(String roleId, String protectedResouceId, String? requestedScopes, CancellationToken cancellationToken)
        {
            if (String.IsNullOrEmpty(requestedScopes))
            {
                var allowedScopes = await _scopeRepository.GetByRoleIdAndProtectedResourceIdAsync(roleId, protectedResouceId, cancellationToken);
                return Result<ICollection<ScopeInfo>>
                    .Success(allowedScopes.Select(scope => scope.ToInfo()).ToList());
            }

            #region Check scopes
            var allowedProtectedResourceScopes = await _scopeRepository.GetByProtectedResourceIdAsync(protectedResouceId, cancellationToken);

            var checkResult = _scopeChecker.CheckScopes(
                sourceScopes: String.Join(' ', allowedProtectedResourceScopes.Select(aScope => aScope.Name)),
                targetScopes: requestedScopes
            );
            if (checkResult is false)
                return Result<ICollection<ScopeInfo>>.Failure("invalid_scope");
            #endregion

            var splittedScopeNames = requestedScopes.Split(' ');
            List<ScopeInfo> grantedScopes = allowedProtectedResourceScopes
                .Where(anAllowedScope => anAllowedScope.RoleId == roleId)
                .Where(anAllowedScope => splittedScopeNames.Contains(anAllowedScope.Name))
                .Select(anAllowedScope => anAllowedScope.ToInfo())
                .ToList();

            return Result<ICollection<ScopeInfo>>.Success(grantedScopes);
        }
    }
}
