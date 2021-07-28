<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SignatureDocumentDetail.ascx.cs" Inherits="RockWeb.Blocks.Core.SignatureDocumentDetail" %>

<asp:UpdatePanel ID="upnlSettings" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfSignatureDocumentId" runat="server" />
        <asp:HiddenField ID="hfSignatureTemplateId" runat="server" />

        <asp:Panel ID="pnlDetails" CssClass="panel panel-block" runat="server" Visible="false">

            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-file-contract"></i> <asp:Literal ID="lTitle" runat="server" /></h1>

                <div class="panel-labels">
                    <Rock:HighlightLabel ID="hlTemplate" runat="server" LabelType="Info" />
                    <Rock:HighlightLabel ID="hlStatusLastUpdated" runat="server" LabelType="Info" />
                </div>
            </div>
            <div class="panel-body">

                <div id="pnlEditDetails" runat="server">

                    <asp:ValidationSummary ID="vsDetails" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" />

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockDropDownList ID="ddlDocumentType" runat="server" Label="Signature Document Type" DataValueField="Id" DataTextField="Name" Required="true" />
                            <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.SignatureDocument, Rock" PropertyName="Name" />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockRadioButtonList ID="rbStatus" runat="server" Label="Status" RepeatDirection="Horizontal"></Rock:RockRadioButtonList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:FileUploader ID="fuDocument" runat="server" Label="Document" />
                            <Rock:RockLiteral ID="lDocumentKey" runat="server" Label="Document Key" />
                            <Rock:RockLiteral ID="lRequestDate" runat="server" Label="Last Invite Date" />
                        </div>
                        <div class="col-md-6">
                            <Rock:PersonPicker ID="ppAppliesTo" runat="server" Label="Applies To" Required="true"
                                Help="The person that this document applies to." />
                            <Rock:PersonPicker ID="ppAssignedTo" runat="server" Label="Assigned To" Required="true"
                                Help="The person that this document was assigned to for getting a signature." />
                            <Rock:PersonPicker ID="ppSignedBy" runat="server" Label="Signed By"
                                Help="The person that signed this." />
                        </div>
                    </div>

                    <div class="actions">
                        <asp:LinkButton ID="btnSave" runat="server" AccessKey="s" ToolTip="Alt+s" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:LinkButton ID="btnCancel" runat="server" AccessKey="c" ToolTip="Alt+c" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                    </div>

                </div>

                <fieldset id="fieldsetViewDetails" runat="server">
                    <Rock:NotificationBox ID="nbErrorMessage" runat="server" Visible="false" />
                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Literal ID="lLeftDetails" runat="server" />
                        </div>
                        <div class="col-md-6">
                            <asp:Literal ID="lRightDetails" runat="server" />
                        </div>
                    </div>
                    <div id="divPDF"></div>
                    <div class="actions">
                        <asp:LinkButton ID="btnEdit" runat="server" AccessKey="e" ToolTip="Alt+e" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" CausesValidation="false" />
                    </div>

                </fieldset>

            </div>

        </asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
