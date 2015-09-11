/// <reference path="../_all.ts" />

/*
    Detail (view/add/edit) controller
*/
module dockyard.controllers {
    'use strict';

    export interface IProcessTemplateListScope extends ng.IScope {
        ptvms: angular.resource.IResourceArray<dockyard.interfaces.IProcessTemplateVM>;
        nav: (pt: interfaces.IProcessTemplateVM) => void,
        remove: (pt: interfaces.IProcessTemplateVM) => void
    }

    /*
        List controller
    */
    class ProcessTemplateListController {
        // $inject annotation.
        // It provides $injector with information about dependencies to be injected into constructor
        // it is better to have it close to the constructor, because the parameters must match in count and type.
        // See http://docs.angularjs.org/guide/di
        public static $inject = [
            '$rootScope',
            '$scope',
            'ProcessTemplateService',
            '$modal',
            '$filter',
            'DTOptionsBuilder',
            'DTColumnBuilder'
        ];

        constructor(
            private $rootScope: interfaces.IAppRootScope,
            private $scope: IProcessTemplateListScope,
            private ProcessTemplateService: services.IProcessTemplateService,
            private $modal,
            private $filter,
            private DTOptionsBuilder,
            private DTColumnBuilder) {

            //Clear the last result value (but still allow time for the confirmation message to show up)
            setTimeout(function () {
                delete $rootScope.lastResult;
            }, 300);

            $scope.$on('$viewContentLoaded', function () {
                // initialize core components
                Metronic.initAjax();
            });

            //Load Process Templates view model
            $scope.ptvms = ProcessTemplateService.query();
            var vm = this; 
            vm.DTOptionsBuilder = DTOptionsBuilder.fromSource('/api/processTemplate')
                .withPaginationType('full_numbers');            

            vm.DTColumnBuilder = [
                DTColumnBuilder.newColumn('Id').withTitle('Id').notVisible(),
                DTColumnBuilder.newColumn('Name').withTitle('Name'),
                DTColumnBuilder.newColumn('Description').withTitle('Description'),
                DTColumnBuilder.newColumn('ProcessTemplateState').withTitle('Status')
                    .renderWith(function (data, type, full, meta) {

                        if (data.ProcessTemplateState === 1) {
                            return '<span class="bold font-green-haze">Inactive</span>'
                        }
                        else {
                            return '<span class="bold font-green-haze">Active</span>'
                        }

                    }),
                DTColumnBuilder.newColumn(null).withTitle('Actions').notSortable()
                    .renderWith(function (data, type, full, meta) {
                        return '<button type="button" class="btn btn-sm red" ng-click="remove(' + data + '); $event.stopPropagation();">Delete</button>'
                    })
            ];            
            
            //Detail/edit link
            $scope.nav = function (pt) {                
                if (pt != null)
                {
                    window.location.href = '#processes/' + pt.id + '/builder';
                }
            }

            //Delete link
            $scope.remove = function (pt) {
                $modal.open({
                    animation: true,
                    templateUrl: 'modalDeleteConfirmation',
                    controller: 'ProcessTemplateListController__DeleteConfirmation',
 
                }).result.then(function (selectedItem) {
                    //Deletion confirmed
                    ProcessTemplateService.delete({ id: pt.id }).$promise.then(function () {
                        $rootScope.lastResult = "success";
                        $scope.ptvms
                        window.location.href = '#processes';
                    });

                    //Remove from local collection
                    $scope.ptvms = $filter('filter')($scope.ptvms, function (value, index) { return value.Id !== pt.id; })

                }, function () {
                    //Deletion cancelled
                });
            };
        }
    }
    app.controller('ProcessTemplateListController', ProcessTemplateListController);

    /*
        A simple controller for Delete confirmation dialog.
        Note: here goes a simple (not really a TypeScript) way to define a controller. 
        Not as a class but as a lambda function.
    */
    app.controller('ProcessTemplateListController__DeleteConfirmation', ($scope: any, $modalInstance: any): void => {
        $scope.ok = function () {
            $modalInstance.close();
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    });
}