<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MediaElementDetail.ascx.cs" Inherits="RockWeb.Blocks.Cms.MediaElementDetail" %>

<script type="text/javascript">
    function clearActiveDialog() {
        $('#<%=hfActiveDialog.ClientID %>').val('');
    }
</script>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>

        <asp:Panel ID="pnlView" runat="server" CssClass="panel panel-block">

            <div class="panel-heading ">
                <h1 class="panel-title"><i class="fas fa-play-circle"></i>
                    <asp:Literal ID="lActionTitle" runat="server" /></h1>
            </div>

            <div class="panel-body">
                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
                <asp:HiddenField ID="hfId" runat="server" />
                <asp:HiddenField ID="hfMediaFolderId" runat="server" />
                <asp:HiddenField ID="hfDisallowManualEntry" runat="server" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" />

                <div id="pnlViewDetails" runat="server">
                    <div class="row margin-b-lg">
                        <div class="col-md-12">
                            <asp:Literal ID="lDescription" runat="server" />
                        </div>
                    </div>
                    <div class="actions">
                        <asp:LinkButton ID="btnEdit" runat="server" AccessKey="e" ToolTip="Alt+e" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" CausesValidation="false" />
                    </div>
                </div>
                <div id="pnlEditDetails" runat="server">
                    <div class="row">
                        <div class="col-md-6">
                            <Rock:DataTextBox ID="tbName" runat="server" SourceTypeName="Rock.Model.MediaElement, Rock" PropertyName="Name" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <Rock:DataTextBox ID="tbDescription" runat="server" SourceTypeName="Rock.Model.MediaElement, Rock" PropertyName="Description" TextMode="MultiLine" Rows="3" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <Rock:NumberBox ID="nbDuration" CssClass="input-width-xl" runat="server" NumberType="Double" Label="Duration" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <label>Media Files</label>
                            <Rock:Grid ID="gMediaFiles" runat="server" EmptyDataText="No Media Files" RowItemText="Media" DisplayType="Light" ShowHeader="true">
                                <Columns>
                                    <Rock:RockBoundField DataField="Quality" HeaderText="Quality" />
                                    <Rock:RockBoundField DataField="Format" HeaderText="Format" />
                                    <Rock:RockBoundField DataField="Dimension" HeaderText="Dimension" />
                                    <Rock:RockBoundField DataField="FormattedFileSize" HeaderText="Size" />
                                    <Rock:BoolField DataField="AllowDownload" HeaderText="Allow Download" SortExpression="AllowDownload" />
                                    <Rock:RockBoundField DataField="Link" HeaderText="Link" />
                                    <Rock:EditField OnClick="gMediaFiles_Edit" />
                                </Columns>
                            </Rock:Grid>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <label>Thumbnail Files</label>
                            <Rock:Grid ID="gThumbnailFiles" runat="server" EmptyDataText="No Media Files" RowItemText="Thumbnail" DisplayType="Light" ShowHeader="true">
                                <Columns>
                                    <Rock:RockBoundField DataField="Dimension" HeaderText="Dimension" />
                                    <Rock:RockBoundField DataField="FormattedFileSize" HeaderText="Size" />
                                    <Rock:RockBoundField DataField="Link" HeaderText="Link" />
                                    <Rock:EditField OnClick="gThumbnailFiles_Edit" />
                                </Columns>
                            </Rock:Grid>
                        </div>
                    </div>
                    <div class="actions">
                        <asp:LinkButton ID="btnSave" runat="server" AccessKey="s" ToolTip="Alt+s" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:LinkButton ID="btnCancel" runat="server" AccessKey="c" ToolTip="Alt+c" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:HiddenField ID="hfActiveDialog" runat="server" />
        <Rock:ModalDialog ID="mdMediaFile" runat="server" Title="File Info" OnSaveClick="mdMediaFile_SaveClick" OnCancelScript="clearActiveDialog();" ValidationGroup="MediaFile">
            <Content>
                <asp:HiddenField ID="hfMediaElementData" runat="server" />
                <asp:ValidationSummary ID="ValidationSummaryMediaFile" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="MediaFile" />
                <div class="row">
                    <div class="col-md-12">
                        <Rock:RockTextBox ID="tbPublicName" runat="server" Label="Public Name" ValidationGroup="MediaFile" Required="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <Rock:UrlLinkBox ID="urlLink" runat="server" Label="Link" ValidationGroup="MediaFile" CssClass="input-width-xxl"  />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <Rock:RockTextBox ID="tbQuality" runat="server" Label="Quality" ValidationGroup="MediaFile" CssClass="input-width-md" Required="true" />
                    </div>
                    <div class="col-md-6">
                        <Rock:RockTextBox ID="tbFormat" runat="server" Label="Format" ValidationGroup="MediaFile" CssClass="input-width-lg" Required="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <Rock:NumberBox ID="nbWidth" runat="server" Label="Width" NumberType="Integer" ValidationGroup="MediaFile" CssClass="input-width-md" Required="true"/>
                    </div>
                    <div class="col-md-6">
                        <Rock:NumberBox ID="nbHeight" runat="server" Label="Height" NumberType="Integer" ValidationGroup="MediaFile" CssClass="input-width-md" Required="true"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <Rock:NumberBox ID="nbFPS" runat="server" Label="FPS" NumberType="Integer" ValidationGroup="MediaFile" CssClass="input-width-md" Required="true" />
                    </div>
                    <div class="col-md-6">
                        <Rock:NumberBox ID="nbSize" runat="server" Label="Size" NumberType="Integer" ValidationGroup="MediaFile" CssClass="input-width-lg" Required="true"/>
                    </div>
                </div>
            </Content>
        </Rock:ModalDialog>
        <Rock:ModalDialog ID="mdThumbnailFile" runat="server" Title="Thumbnail File Info" OnSaveClick="mdThumbnailFile_SaveClick" OnCancelScript="clearActiveDialog();" ValidationGroup="ThumbnailFile">
            <Content>
                <asp:HiddenField ID="hfThumbnailFile" runat="server" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" HeaderText="Please correct the following:" CssClass="alert alert-validation" ValidationGroup="ThumbnailFile" />
                <div class="row">
                    <div class="col-md-12">
                        <Rock:UrlLinkBox ID="urlThumbnailLink" runat="server" Label="Link" ValidationGroup="ThumbnailFile" CssClass="input-width-xxl" Required="true" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <Rock:NumberBox ID="nbThumbnailWidth" runat="server" Label="Width" NumberType="Integer" ValidationGroup="ThumbnailFile" CssClass="input-width-md" Required="true"/>
                    </div>
                    <div class="col-md-6">
                        <Rock:NumberBox ID="nbThumbnailHeight" runat="server" Label="Height" NumberType="Integer" ValidationGroup="ThumbnailFile" CssClass="input-width-md" Required="true"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <Rock:NumberBox ID="nbThumbnailSize" runat="server" Label="Size" NumberType="Integer" ValidationGroup="ThumbnailFile" CssClass="input-width-lg" Required="true"/>
                    </div>
                </div>
            </Content>
        </Rock:ModalDialog>
    </ContentTemplate>
</asp:UpdatePanel>
