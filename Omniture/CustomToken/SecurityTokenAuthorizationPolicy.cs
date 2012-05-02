using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace Omniture.CustomToken
{
    internal class SecurityTokenAuthorizationPolicy : IAuthorizationPolicy
    {
        #region Fields

        private readonly string _id;
        private readonly ClaimSet _issuer;
        private readonly IEnumerable<ClaimSet> _issuedClaimSets;

        #endregion

        #region Properties

        public ClaimSet Issuer { get { return _issuer; } }
        public string Id { get { return _id; } }

        #endregion

        #region Constructors

        internal SecurityTokenAuthorizationPolicy( ClaimSet issuedClaims )
        {
            if ( issuedClaims == null )
                throw new ArgumentNullException( "issuedClaims" );

            _issuer = issuedClaims.Issuer;
            _issuedClaimSets = new[] { issuedClaims };
            _id = Guid.NewGuid().ToString();
        }

        #endregion

        #region IAuthorizationPolicy Members

        bool IAuthorizationPolicy.Evaluate( EvaluationContext evaluationContext, ref object state )
        {
            foreach ( ClaimSet issuance in _issuedClaimSets )
                evaluationContext.AddClaimSet( this, issuance );

            return true;
        }

        #endregion
    }
}
