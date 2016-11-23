﻿angular.module('virtoCommerce.sitemapsModule')
.controller('virtoCommerce.sitemapsModule.sitemapController', ['$scope', 'platformWebApp.bladeUtils', 'platformWebApp.bladeNavigationService', 'platformWebApp.uiGridHelper', 'virtoCommerce.sitemapsModule.sitemaps', function ($scope, bladeUtils, bladeNavigationService, uiGridHelper, sitemapsResource) {
    bladeUtils.initializePagination($scope);
    var blade = $scope.blade;
    blade.headIcon = 'fa fa-sitemap';
    blade.isLoading = false;
    blade.toolbarCommands = getBladeToolbarCommands();
    blade.refresh = function () {
        searchSitemapItems(blade.currentEntity.id, ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount, $scope.pageSettings.itemsPerPageCount);
    }
    blade.removeSitemapItems = function (items) {
        removeSitemapItems(blade.currentEntity, _.map(items, function (i) { return i.id }));
    }

    $scope.setForm = function (form) {
        $scope.formScope = form;
    }

    $scope.setGridOptions = function (gridOptions) {
        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
        });
        bladeUtils.initializePagination($scope);
    }

    function getBladeToolbarCommands() {
        return [{
            name: 'sitemapsModule.blades.sitemap.toolbar.addItems',
            icon: 'fa fa-plus',
            canExecuteMethod: function () {
                return true;
            },
            executeMethod: function () {
                showAddItemsBlade($scope.gridApi.selection.getSelectedRows());
            }
        }, {
            name: 'sitemapsModule.blades.sitemap.toolbar.removeItems',
            icon: 'fa fa-trash',
            canExecuteMethod: function () {
                return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
            },
            executeMethod: function () {
                blade.removeSitemapItems($scope.gridApi.selection.getSelectedRows());
            }
        }, {
            name: 'sitemapsModule.blades.sitemap.toolbar.saveSitemap',
            icon: 'fa fa-save',
            canExecuteMethod: function () {
                return $scope.formScope && $scope.formScope.$valid;
            },
            executeMethod: function () {
                if (blade.currentEntity.isNew) {
                    addSitemap(blade.currentEntity);
                } else {
                    updateSitemap(blade.currentEntity);
                }
            }
        }];
    }

    function showAddItemsBlade(selectedItems) {
        var addItemsBlade = {
            id: 'addSitemapItemsBlade',
            title: 'sitemapsModule.blades.addItems.title',
            controller: 'virtoCommerce.sitemapsModule.sitemapItemsAddController',
            template: 'Modules/$(VirtoCommerce.Sitemaps)/Scripts/blades/sitemap-items-add.tpl.html',
            selectedItems: selectedItems
        }
        bladeNavigationService.showBlade(addItemsBlade, blade);
    }

    function addSitemap(sitemap) {
        blade.isLoading = true;
        sitemapsResource.addSitemap({}, sitemap, function (response) {
            blade.parentBlade.refresh();
            blade.isLoading = false;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
            blade.isLoading = false;
        });
    }

    function updateSitemap(sitemap) {
        blade.isLoading = true;
        sitemapsResource.updateSitemap({}, sitemap, function (response) {
            blade.isLoading = false;
            blade.parentBlade.refresh();
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
            blade.isLoading = false;
        });
    }

    function searchSitemapItems(sitemapId, skip, take) {
        blade.isLoading = true;
        sitemapsResource.searchSitemapItems({}, {
            sitemapId: sitemapId,
            skip: skip,
            take: take
        }, function (response) {
            $scope.pageSettings.totalItems = response.totalCount;
            blade.currentEntity.items = response.items;
            blade.isLoading = false;
        }, function (error) {
            bladeNavigationService.setError('Error ' + error.status, blade);
            blade.isLoading = false;
        });
    }

    function removeSitemapItems(sitemap, itemIds) {
        if (sitemap.isNew) {
        } else {
            blade.isLoading = true;
            sitemapsResource.removeSitemapItems({
                sitemapId: sitemap.id,
                itemIds: itemIds
            }, function (response) {
                blade.refresh();
                blade.isLoading = false;
            }, function (error) {
                bladeNavigationService.setError('Error ' + error.status, blade);
                blade.isLoading = false;
            });
        }
    }
}]);