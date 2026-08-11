namespace Gym.AuthorizationServer.Services
{
    public interface IScopeChecker
    {
        Boolean CheckScopes(String? sourceScopes, String? targetScopes);
    }

    public class ScopeChecker : IScopeChecker
    {
        public Boolean CheckScopes(String? sourceScopes, String? targetScopes)
        {
            if (String.IsNullOrWhiteSpace(sourceScopes) && String.IsNullOrWhiteSpace(targetScopes))
                return true;

            if (String.IsNullOrWhiteSpace(sourceScopes) || String.IsNullOrWhiteSpace(targetScopes))
                return false;

            var splittedSourceScopes = sourceScopes.Split(' ');
            var splittedTargetScopes = targetScopes.Split(' ');

            Boolean isDifferent = this.DifferenceExists(splittedSourceScopes, splittedTargetScopes);
            if(isDifferent) 
                return false;
            return true;
        }
        private Boolean DifferenceExists(IEnumerable<String> sourceScopes, IEnumerable<String> targetScopes)
        {
            var difference = targetScopes.Except(sourceScopes).ToArray();
            if (difference.Any())
                return true;
            return false;
        }
    }
}
