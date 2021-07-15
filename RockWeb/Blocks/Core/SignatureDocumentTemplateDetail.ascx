<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SignatureDocumentTemplateDetail.ascx.cs" Inherits="RockWeb.Blocks.Core.SignatureDocumentTemplateDetail" %>

<asp:UpdatePanel ID="upnlSettings" runat="server">
    <ContentTemplate>
        <asp:HiddenField ID="hfSignatureDocumentTemplateId" runat="server" />

        <asp:Panel ID="pnlDetails" CssClass="panel panel-block" runat="server" Visible="false">

            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-file-contract"></i> <asp:Literal ID="lTitle" runat="server" /></h1>
                 <div class="panel-labels">
                    <Rock:HighlightLabel ID="hlInactive" runat="server" LabelType="Danger" Text="Inactive" />
                </div>
            </div>
            <div class="panel-body">

                <div id="pnlEditDetails" runat="server">

                    <asp:ValidationSummary ID="vsDetails" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" />
                    <Rock:NotificationBox ID="nbError" runat="server" Text="Error Occurred trying to retrieve templates" NotificationBoxType="Danger" Visible="false"></Rock:NotificationBox>

                    <div class="row">
                        <div class="col-md-6">
                            <Rock:DataTextBox ID="tbTypeName" runat="server" SourceTypeName="Rock.Model.SignatureDocumentTemplate, Rock" PropertyName="Name" />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockCheckBox ID="cbIsActive" runat="server" Label="Active" />
                        </div>
                    </div>
                    <Rock:DataTextBox ID="tbTypeDescription" runat="server" SourceTypeName="Rock.Model.SignatureDocumentTemplate, Rock" PropertyName="Description" TextMode="MultiLine" Rows="3" ValidateRequestMode="Disabled" />
                    <div class="row">
                        <div class="col-md-6">
                            <Rock:DataTextBox ID="tbDocumentTerm" runat="server" SourceTypeName="Rock.Model.SignatureDocumentTemplate, Rock" PropertyName="DocumentTerm" Help="How the document should be referred to (e.g. Wavier, Contract, Statement, etc.)" />
                        </div>
                        <div class="col-md-6">
                            <Rock:BinaryFileTypePicker ID="bftpDocumentFileType" runat="server" Label="Document File Type" Required="true" Help="Determine which file type is used when storing the signed document." />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <Rock:RockRadioButtonList ID="rrbSignatureInputType" runat="server" Label="Signature Input Type" RepeatDirection="Horizontal" />
                        </div>
                        <div class="col-md-6">
                            <Rock:RockDropDownList ID="ddlCompletionEmailTemplate" runat="server" Label="Completion Email Template" Required="true" Visible="false"
                                Help="The email template to use when sending the signed document upon completion." />
                        </div>
                    </div>
                      <div class="row">
                          <div class="col-md-12">
                              <a href="#" class="btn btn-xs btn-link js-show-template-tips pull-right">Template Tips</a>
                          </div>
                      </div>
                    <div class="well js-slidingtemplatetips-help margin-v-none" style="display: none">
                        <h2>Template Tips</h2>
                        <p>Below are some tips to assist you in your creation. The merge fields that you use to customize your templates will vary depending on where they are being used.
                            The most common merge fields are shown below.</p>
                        <p>Be sure to add the {{ SignatureInformation }} merge field where you would like the signature information to be displayed. if you do not provide this merge field it will be added for you automatically at the end of the template.</p>
                    </div>
                    <Rock:CodeEditor ID="ceLavaTemplate" runat="server" Label="Lava Template" Help="The lava template that makes up the body of the document." />
                    <asp:Panel ID="pnlLegacySetting" runat="server" CssClass="well" Visible="false">
                        <h4>Legacy Signature Provider Settings</h4>
                        <p>Support for these providers will be fully removed in next full release.</p>
                        <hr/>
                        <div class="row">
                            <div class="col-md-6">
                                <Rock:ComponentPicker ID="cpProvider" runat="server" ContainerType="Rock.Security.DigitalSignatureContainer, Rock" Label="Digital Signature Provider" OnSelectedIndexChanged="cpProvider_SelectedIndexChanged" AutoPostBack="true" Required="true" />
                                
                            </div>
                            <div class="col-md-6">
                                <Rock:RockDropDownList ID="ddlTemplate" runat="server" Label="Template" Help="A template that has been created with your digital signature provider" Required="true" />
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="actions">
                        <asp:LinkButton ID="btnSaveType" runat="server" AccessKey="s" ToolTip="Alt+s" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveType_Click" />
                        <asp:LinkButton ID="btnCancelType" runat="server" AccessKey="c" ToolTip="Alt+c" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancelType_Click" />
                    </div>

                </div>

                <fieldset id="fieldsetViewDetails" runat="server">

                    <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />

                    <div class="row margin-b-md">
                        <div class="col-md-12">
                            <asp:Literal ID="lDescription" runat="server" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <asp:Literal ID="lLeftDetails" runat="server" />
                        </div>
                        <div class="col-md-6">
                            <asp:Literal ID="lRightDetails" runat="server" />
                        </div>
                    </div>

                    <div class="actions">
                        <asp:LinkButton ID="btnEdit" runat="server" AccessKey="m" ToolTip="Alt+m" Text="Edit" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnEdit_Click" />
                        <Rock:ModalAlert ID="mdDeleteWarning" runat="server" />
                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-link" OnClick="btnDelete_Click" CausesValidation="false" />
                    </div>

                </fieldset>

            </div>

        </asp:Panel>
        <script type="text/javascript">
            Sys.Application.add_load(function () {
                $('.js-show-template-tips').off('click').on('click', function () {
                    $('.js-slidingtemplatetips-help').slideToggle();
                    return false;
                });
            });
        </script>
    </ContentTemplate>
</asp:UpdatePanel>
