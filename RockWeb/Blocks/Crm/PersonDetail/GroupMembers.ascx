﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupMembers.ascx.cs" Inherits="RockWeb.Blocks.Crm.PersonDetail.GroupMembers" %>

<Rock:RockUpdatePanel ID="upGroupMembers" runat="server">
    <ContentTemplate>
        <div class="persondetails-grouplist js-grouplist-sort-container">

            <asp:Repeater ID="rptrGroups" runat="server" OnItemDataBound="rptrGroups_ItemDataBound" >
                <ItemTemplate>

                    <asp:Panel ID="pnlGroup" runat="server" CssClass="card card-profile group-hover card-family-member">
                        <asp:HiddenField ID="hfGroupId" runat="server" Value='<%# Eval("Id") %>' />
                            <a id="lReorderIcon" runat="server" class="btn btn-link btn-xs panel-widget-reorder align-self-center pull-left js-stop-immediate-propagation"><i class="fa fa-bars"></i></a>
                            <div class="card-header">
                                <span class="card-title"><%# FormatAsHtmlTitle(Eval("Name").ToString()) %></span>

                                <div class="panel-labels group-hover-item group-hover-show">
                                    <asp:HyperLink ID="hlEditGroup" runat="server" AccessKey="O" ToolTip="Alt+O" CssClass="btn btn-link btn-xs"><i class="fa fa-pencil"></i></asp:HyperLink>
                                </div>
                            </div>

                        <asp:Literal ID="lGroupHeader" runat="server" />

                        <div class="card-section pb-0">
                            <div class="d-flex flex-wrap">
                                <asp:Repeater ID="rptrMembers" runat="server" OnItemDataBound="rptrMembers_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:Literal ID="litGroupMemberInfo" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>

                        <asp:panel ID="pnlGroupAttributes" runat="server" CssClass="card-section">
                            <dl class="m-0">
                                <asp:Literal ID="litGroupAttributes" runat="server"></asp:Literal>
                            </dl>
                        </asp:panel>

                        <div class="card-section">
                            <asp:Repeater ID="rptrAddresses" runat="server" OnItemDataBound="rptrAddresses_ItemDataBound">
                                <ItemTemplate>
                                    <asp:Literal ID="litAddress" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <asp:Literal ID="lGroupFooter" runat="server" />

                    </asp:Panel>

                </ItemTemplate>
            </asp:Repeater>
        </div>

        <script>
            Sys.Application.add_load(function () {

                var fixHelper = function (e, ui) {
                    ui.children().each(function () {
                        $(this).width($(this).width());
                    });
                    return ui;
                };

                // javascript to make the Reorder buttons work on the panel-widget controls
                $('.js-grouplist-sort-container').sortable({
                    helper: fixHelper,
                    handle: '.panel-widget-reorder',
                    containment: 'parent',
                    tolerance: 'pointer',
                    start: function (event, ui) {
                        {
                            var start_pos = ui.item.index();
                            ui.item.data('start_pos', start_pos);
                        }
                    },
                    update: function (event, ui) {
                        {
                            var newItemIndex = $(ui.item).prevAll('.panel-widget').length;
                            var postbackArg = 're-order-panel-widget:' + ui.item.attr('id') + ';' + newItemIndex;
                            window.location = "javascript:__doPostBack('<%=upGroupMembers.ClientID %>', '" +  postbackArg + "')";
                        }
                    }
                });
            });
        </script>

    </ContentTemplate>
</Rock:RockUpdatePanel>