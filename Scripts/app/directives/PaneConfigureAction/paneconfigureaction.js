var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="../../_all.ts" />
var dockyard;
(function (dockyard) {
    var directives;
    (function (directives) {
        var paneConfigureAction;
        (function (paneConfigureAction) {
            'use strict';
            (function (MessageType) {
                MessageType[MessageType["PaneConfigureAction_ActionUpdated"] = 0] = "PaneConfigureAction_ActionUpdated";
                MessageType[MessageType["PaneConfigureAction_ActionRemoved"] = 1] = "PaneConfigureAction_ActionRemoved";
                MessageType[MessageType["PaneConfigureAction_InternalAuthentication"] = 2] = "PaneConfigureAction_InternalAuthentication";
                MessageType[MessageType["PaneConfigureAction_ExternalAuthentication"] = 3] = "PaneConfigureAction_ExternalAuthentication";
            })(paneConfigureAction.MessageType || (paneConfigureAction.MessageType = {}));
            var MessageType = paneConfigureAction.MessageType;
            var ActionUpdatedEventArgs = (function (_super) {
                __extends(ActionUpdatedEventArgs, _super);
                function ActionUpdatedEventArgs() {
                    _super.apply(this, arguments);
                }
                return ActionUpdatedEventArgs;
            })(directives.ActionUpdatedEventArgsBase);
            paneConfigureAction.ActionUpdatedEventArgs = ActionUpdatedEventArgs;
            var InternalAuthenticationArgs = (function () {
                function InternalAuthenticationArgs(activityTemplateId) {
                    this.activityTemplateId = activityTemplateId;
                }
                return InternalAuthenticationArgs;
            })();
            paneConfigureAction.InternalAuthenticationArgs = InternalAuthenticationArgs;
            var ExternalAuthenticationArgs = (function () {
                function ExternalAuthenticationArgs(activityTemplateId) {
                    this.activityTemplateId = activityTemplateId;
                }
                return ExternalAuthenticationArgs;
            })();
            paneConfigureAction.ExternalAuthenticationArgs = ExternalAuthenticationArgs;
            var RenderEventArgs = (function () {
                function RenderEventArgs(action) {
                    // Clone Action to prevent any issues due to possible mutation of source object
                    this.action = angular.extend({}, action);
                }
                return RenderEventArgs;
            })();
            paneConfigureAction.RenderEventArgs = RenderEventArgs;
            var MapFieldsClickedEventArgs = (function () {
                function MapFieldsClickedEventArgs(action) {
                    this.action = action;
                }
                return MapFieldsClickedEventArgs;
            })();
            paneConfigureAction.MapFieldsClickedEventArgs = MapFieldsClickedEventArgs;
            var ActionRemovedEventArgs = (function () {
                function ActionRemovedEventArgs(id, isTempId) {
                    this.id = id;
                    this.isTempId = isTempId;
                }
                return ActionRemovedEventArgs;
            })();
            paneConfigureAction.ActionRemovedEventArgs = ActionRemovedEventArgs;
            var CancelledEventArgs = (function (_super) {
                __extends(CancelledEventArgs, _super);
                function CancelledEventArgs() {
                    _super.apply(this, arguments);
                }
                return CancelledEventArgs;
            })(directives.CancelledEventArgsBase);
            paneConfigureAction.CancelledEventArgs = CancelledEventArgs;
            //More detail on creating directives in TypeScript: 
            //http://blog.aaronholmes.net/writing-angularjs-directives-as-typescript-classes/
            var PaneConfigureAction = (function () {
                function PaneConfigureAction(ActionService, crateHelper, $filter, $timeout) {
                    this.ActionService = ActionService;
                    this.crateHelper = crateHelper;
                    this.$filter = $filter;
                    this.$timeout = $timeout;
                    this.templateUrl = '/AngularTemplate/PaneConfigureAction';
                    this.scope = {
                        currentAction: '='
                    };
                    this.restrict = 'E';
                    PaneConfigureAction.prototype.link = function (scope, element, attrs) {
                        //Link function goes here
                    };
                    PaneConfigureAction.prototype.controller = function ($scope, $element, $attrs) {
                        $scope.$on("onChange", onControlChange);
                        // Get configuration settings template from the server if the current action does not 
                        // contain those or user has selected another action template.
                        //if (scope.currentAction.crateStorage == null
                        //    || scope.currentAction.configurationControls.fields == null
                        //    || scope.currentAction.configurationControls.fields.length == 0
                        //    || (eventArgs.action.id == this._currentAction.id &&
                        //        eventArgs.action.actionTemplateId != this._currentAction.actionTemplateId)) {
                        //FOR NOW we're going to simplify things by always checking with this server for a new configuration                
                        if ($scope.currentAction.activityTemplateId > 0) {
                            loadConfiguration();
                        }
                        $scope.onConfigurationChanged = function (newValue, oldValue) {
                            if (!newValue || !newValue.fields || newValue.fields === oldValue.fields || newValue.fields.length == 0)
                                return;
                            crateHelper.mergeControlListCrate($scope.currentAction.configurationControls, $scope.currentAction.crateStorage);
                            $scope.currentAction.crateStorage.crateDTO = $scope.currentAction.crateStorage.crates; //backend expects crates on CrateDTO field
                            ActionService.save({ id: $scope.currentAction.id }, $scope.currentAction, null, null);
                        };
                        function onControlChange(event, eventArgs) {
                            var scope = event.currentScope;
                            // Check if this event is defined for the current field
                            var fieldName = eventArgs.fieldName;
                            var fieldList = scope.currentAction.configurationControls.fields;
                            // Find the configuration field object for which the event has fired
                            fieldList = $filter('filter')(fieldList, { name: fieldName }, true);
                            if (fieldList.length == 0 || !fieldList[0].events || fieldList[0].events.length == 0)
                                return;
                            var field = fieldList[0];
                            // Find the onChange event object
                            var eventHandlerList = $filter('filter')(field.events, { name: 'onChange' }, true);
                            if (eventHandlerList.length == 0)
                                return;
                            var fieldEvent = eventHandlerList[0];
                            if (fieldEvent.handler === 'requestConfig') {
                                crateHelper.mergeControlListCrate(scope.currentAction.configurationControls, scope.currentAction.crateStorage);
                                scope.currentAction.crateStorage.crateDTO = scope.currentAction.crateStorage.crates; //backend expects crates on CrateDTO field
                                loadConfiguration();
                            }
                        }
                        // Here we look for Crate with ManifestType == 'Standard Configuration Controls'.
                        // We parse its contents and put it into currentAction.configurationControls structure.
                        function loadConfiguration() {
                            // Block pane and show pane-level 'loading' spinner
                            $scope.processing = true;
                            var activityTemplateName = $scope.currentAction.activityTemplateName; // preserve activity name
                            if ($scope.configurationWatchUnregisterer) {
                                $scope.configurationWatchUnregisterer();
                            }
                            ActionService.configure($scope.currentAction).$promise.then(function (res) {
                                // Check if authentication is required.
                                if (crateHelper.hasCrateOfManifestType(res.crateStorage, 'Standard Authentication')) {
                                    var authCrate = crateHelper
                                        .findByManifestType(res.crateStorage, 'Standard Authentication');
                                    var authMS = angular.fromJson(authCrate.contents);
                                    // Dockyard auth mode.
                                    if (authMS.Mode == 1) {
                                        $scope.$emit(MessageType[MessageType.PaneConfigureAction_InternalAuthentication], new InternalAuthenticationArgs(res.activityTemplateId));
                                    }
                                    else {
                                        // self.$window.open(authMS.Url, '', 'width=400, height=500, location=no, status=no');
                                        $scope.$emit(MessageType[MessageType.PaneConfigureAction_ExternalAuthentication], new ExternalAuthenticationArgs(res.activityTemplateId));
                                    }
                                    $scope.processing = false;
                                    return;
                                }
                                // Unblock pane
                                $scope.processing = false;
                                // Assign name to res rather than currentAction to prevent 
                                // $watches from unnecessarily triggering
                                res.activityTemplateName = activityTemplateName;
                                $scope.currentAction.crateStorage = res.crateStorage;
                                $scope.currentAction.configurationControls =
                                    crateHelper.createControlListFromCrateStorage($scope.currentAction.crateStorage);
                                $timeout(function () {
                                    $scope.configurationWatchUnregisterer = $scope.$watch(function (scope) { return $scope.currentAction.configurationControls; }, $scope.onConfigurationChanged, true);
                                }, 1000);
                            });
                        }
                        ;
                    };
                }
                //The factory function returns Directive object as per Angular requirements
                PaneConfigureAction.Factory = function () {
                    var directive = function (ActionService, crateHelper, $filter, $timeout) {
                        return new PaneConfigureAction(ActionService, crateHelper, $filter, $timeout);
                    };
                    directive['$inject'] = ['ActionService', 'CrateHelper', '$filter', '$timeout'];
                    return directive;
                };
                return PaneConfigureAction;
            })();
            app.directive('paneConfigureAction', PaneConfigureAction.Factory());
        })(paneConfigureAction = directives.paneConfigureAction || (directives.paneConfigureAction = {}));
    })(directives = dockyard.directives || (dockyard.directives = {}));
})(dockyard || (dockyard = {}));
//# sourceMappingURL=paneconfigureaction.js.map