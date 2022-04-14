// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// FinancialPersonSavedAccount SaveHook
    /// </summary>
    public partial class FinancialPersonSavedAccount
    {
        /// <inheritdoc/>
        internal class SaveHook : EntitySaveHook<FinancialPersonSavedAccount>
        {
            /// <inheritdoc/>
            protected override void PreSave()
            {
                var rockContext = ( RockContext ) DbContext;

                switch ( Entry.State )
                {
                    case EntityContextState.Deleted:
                        {
                            // If a FinancialPaymentDetail was linked to this FinancialPersonSavedAccount and is now orphaned, delete it.
                            var financialPaymentDetailService = new FinancialPaymentDetailService( rockContext );
                            financialPaymentDetailService.DeleteOrphanedFinancialPaymentDetail( Entry );

                            break;
                        }
                }

                base.PreSave();
            }

            /// <inheritdoc/>
            protected override void PostSave()
            {
                var rockContext = ( RockContext ) DbContext;
                var paymentDetailId = Entity.FinancialPaymentDetailId;
                FinancialPaymentDetail paymentDetail = null;

                if ( paymentDetailId.HasValue )
                {
                    paymentDetail = new FinancialPaymentDetailService( rockContext ).Get( paymentDetailId.Value );
                }

                if ( paymentDetail != null && paymentDetail.FinancialPersonSavedAccountId == null )
                {
                    // If this FinancialPersonSavedAccount is associated with a FinancialPaymentDetail entity, and that
                    // FinancialPaymentDetail entity is not already associated with another FinancialPersonSavedAccount,
                    // then we should create the reverse-association so that the payment detail points back to this
                    // saved account.  Doing so creates more useful data if the FinancialPaymentDetail entity is cloned
                    // in the future (i.e., because of tokenized payment methods being reused for new scheduled
                    // transactions).
                    paymentDetail.FinancialPersonSavedAccountId = Entity.Id;
                    rockContext.SaveChanges();
                }

                base.PreSave();
            }
        }
    }
}