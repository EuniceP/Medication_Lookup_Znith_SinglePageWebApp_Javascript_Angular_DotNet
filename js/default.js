(function () {
    angular
        .module("pharmaLookup", ['ngSanitize'])
        .filter('DisplayNullCurrency', DisplayNullCurrency)
        .filter('DisplayNullDate', DisplayNullDate)
        .service("DataService", DataService)
        .controller("PharmaCtrl", PharmaCtrl);

    //-----------------------------------------------------
    //  Angular Filter to format Currency value
    //-----------------------------------------------------
    function DisplayNullCurrency ($filter) {
        return function (input) {
            if (!input) { return ' - '; }
            if (parseFloat(input) != 0) {
                return $filter('currency')(input);
            }
            return ' - ';
        };
    };
    //-----------------------------------------------------
    //  Angular Filter to format date value
    //-----------------------------------------------------
    function DisplayNullDate($filter) {
        return function (input) {
            if (!input) { return ' - '; }
            if (input != '01/01/1900') {
                return $filter('date')(input);
            }
            return ' - ';
        };
    };

    //-----------------------------------------------------
    //  Angular Service for HTTP data retrieval from the web service
    //-----------------------------------------------------
    function DataService($http, $timeout) {
        var URL = "/PHARMA_LOOKUP/Service/Service.svc/";
        var httpParm = {
            method: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        };

        this.Pharma_Group = function () {
            httpParm.url = URL + "GET_PHARMA_GROUP";
            return $http(httpParm);
        };
        this.Pharma_GroupClass = function () {
            httpParm.url = URL + "GET_PHARMA_GROUPCLASS";
            return $http(httpParm);
        };
        this.Pharma_Class = function (param) {
            httpParm.url = URL + "GET_PHARMA_CLASS";
            httpParm.params = param;
            return $http(httpParm);
        };
        this.Pharma_Price_History = function (param) {
            httpParm.url = URL + "GET_PHARMA_PRICE_HISTORY";
            httpParm.params = param;
            return $http(httpParm);
        };
        this.Pharma_Ingredient = function (param) {
            httpParm.url = URL + "GET_PHARMA_INGREDIENT";
            httpParm.params = param;
            return $http(httpParm);
        };
        this.Pharma_GenericNames = function (param) {
            httpParm.url = URL + "GET_PHARMA_GENERICNAMES";
            httpParm.params = param;
            return $http(httpParm);
        };
        this.Pharma_Manufacturer = function () {
            httpParm.url = URL + "GET_PHARMA_MANUFACTURER";
            return $http(httpParm);
        };
        this.Pharma_Result = function (param) {
            httpParm.url = URL + "GET_PHARMA_RESULT";
            httpParm.params = param;
            return $http(httpParm);
        };
    };

    //-----------------------------------------------------
    //  Angular Controller: PharmaCtrl
    //-----------------------------------------------------
    function PharmaCtrl($scope, DataService, $timeout) {

        //-------------------------------------------
        // Main function when the form loads
        $scope.init = function() {
            
            // Initialize various variables
            $scope.resetVariables();            

            // Pull Therapeutic Groups and display
            DataService.Pharma_Group()
                .then(function (p1) {
                    $scope.pharmaGroups = p1.data;
                },
                    function (errorP1) {
                        $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp Error loading pharmaceutical groups';
                    }
                );

            // Pull Therapeutic Group & Class names for auto-complete functionality when 'Therapeutic Group/Class Lookup' option is selected
            DataService.Pharma_GroupClass()
                .then(function (p1) {
                    $scope.pharmaGroupClass = p1.data;

                    // Convert JSON to Array for jQuery autocomplete to function
                    arr = Object.keys(p1.data).map(function (k) { return p1.data[k] });

                    $scope.groupClassArray = [];
                    for (var i = 0; i <  arr.length; i++) {
                        $scope.groupClassArray.push(arr[i].name);
                    }
                   
                    $("#therapeutic-groupclass").autocomplete({
                        source: $scope.groupClassArray,
                        minLength: 1,
                        maxLength: 50,
                        delay: 100
                    });
                },
                    function (errorP1) {
                        $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp Error loading pharmaceutical groups/classes';
                    }
                );
        };

        //-------------------------------------------
        // Initialize various variables
        $scope.resetVariables = function () {
            $scope.ndc = '';
            $scope.gpi = '';
            $scope.product = '';
            $scope.groupCode = '';
            $scope.classCode = '';
            $scope.manufacturer = '';
            $scope.typedGroupClass = '';
            $scope.displayMessage = '';
            $scope.errorMessage = '';
            $scope.showReport = false; 
            $scope.predicate = 'name'; // default sort order = drug name
            $scope.reverse = true;
            $scope.showGenericNames = false;

            $('#rda_search_type_PRODUCT').prop("checked", true);  // default search type = PRODUCT
            $scope.changeRadioSelection('PRODUCT');

            // showColumns is used to make the columns with value = true visible initially
            // When a user checks off a column, the value is set to false
            $scope.showColumns = {
                NDC: true, Name: true, GPI: true, Generic_Name: true, Rx_OTC: true, Source: true, Strength: true, Manufacturer: true,
                Repackage_Code: false, Pkg_Size: true, Measure_Unit: true, Unit_Price: true, Pkg_Qty: true, Pkg_Price: true, Pkg_Count: true,
                Price_Effective_Date: true, GEAP: false, Older_Unit_Price: false, Older_Pkg_Price: false, Older_Price_Effective_Date: false,
                DEA: true, Dosage_Form: true, Therapeutic_Group: true, Therapeutic_Class: true, Name_Type: false
            };
            // columnVisibleArray is passed to Export2Excel.js to export only visible columns from DOM
            $scope.columnVisibleArray = $.map($scope.showColumns, function (boolvalue) { return boolvalue });
        };

        //----------------------------------
        // Get an array of showColumns values to be passed to Export2Excel.js to export only visible columns from DOM 
        $scope.getColulmnVisiblilityArray = function () {
            return $.map($scope.showColumns, function (boolvalue) { return boolvalue });
        };

        //----------------------------------
        // Sort columns using column name in predicate - called from Default.aspx via ng-click
        $scope.sortData = function (predicate) {
            closeChildTable();  // if any ingredient or price history child table is opened, close it
            $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
            $scope.predicate = predicate;
        };

        //----------------------------------
        // Called from Default.aspx via ng-click when a Therapetuc Group is selected
        $scope.getTherapeuticClasses = function (groupCode) {
            var params = { Group_Code: groupCode };
            $scope.classCode = '';
            $scope.groupCode = groupCode;
            DataService.Pharma_Class(params)
                .then(function (p1) {
                    $scope.pharmaClasses = p1.data;
                },
                      function (errorP1) {
                          $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp Error loading pharmaceutical classes';
                      }
                );
        };
       
        //----------------------------------
        // Save classCode when user clicks on Therapeutic Class
        $scope.saveTherapeuticClass = function (classCode) {
            $scope.classCode = classCode;
        };

        //----------------------------------
        // Called when user changes radio button selection for search type.  The values set determine the visibility of the corresponding input boxes.
        $scope.changeRadioSelection = function (radioSelected) {
            if (radioSelected == 'SELECTBOX') {
                $('#group-class-selectionboxes').show();
                $('#frmSearch').hide();
                $('#drugGroup').prop("disabled", false);
                $('#drugClass').prop("disabled", false);
            }
            else {
                $('#group-class-selectionboxes').hide();
                $('#frmSearch').show();
                $('#input_box').show();
                $('#drugGroup').prop("disabled", true);
                $('#drugClass').prop("disabled", true);
                $('select#drugGroup option').removeAttr("selected");
                $('select#drugClass option').removeAttr("selected");
            }

            $scope.radioSelected = radioSelected;
            if (radioSelected == 'PRODUCT') {
                $scope.ndc = '';
                $scope.gpi = '';
                $scope.manufacturer = '';
                $scope.typedGroupClass = '';
            }
            else if (radioSelected == 'NDC') {
                $scope.product = '';
                $scope.gpi = '';
                $scope.manufacturer = '';
                $scope.typedGroupClass = '';
            }
            else if (radioSelected == 'GPI') {
                $scope.ndc = '';
                $scope.product = '';
                $scope.manufacturer = '';
                $scope.typedGroupClass = '';
            }
            else if (radioSelected == 'MANUFACTURER') {
                $scope.ndc = '';
                $scope.product = '';
                $scope.gpi = '';
                $scope.typedGroupClass = '';
            }
            else if (radioSelected == 'PHARMCLASS') {
                $scope.ndc = '';
                $scope.gpi = '';
                $scope.product = '';
                $scope.manufacturer = '';
            }
            else if (radioSelected == 'SELECTBOX') {
                $scope.ndc = '';
                $scope.gpi = '';
                $scope.product = '';
                $scope.manufacturer = '';
                $scope.typedGroupClass = '';
            };
        };

        //----------------------------------
        // Reset to not display any price history
        function resetShowPriceHistoryFlags() {
            $scope.showPriceHistory = [];
            for (var i = 0; i < $scope.pharmaResult.length; i += 1) {
                $scope.showPriceHistory.push(false);
            }
        };

        //----------------------------------
        // Display price history of the ndc under cell with index
        $scope.displayPriceHistory = function (index, ndc) {
            if (typeof $scope.showPriceHistory === 'undefined') {
                resetShowPriceHistoryFlags();
            }

            if ($scope.tableRowExpanded === false && $scope.tableRowIndexExpandedCurr === "" && $scope.ndcExpanded === "") {
                $scope.tableRowIndexExpandedPrev = "";
                $scope.tableRowExpanded = true;
                $scope.tableRowIndexExpandedCurr = index;
                $scope.ndcExpanded = ndc;
                $scope.showPriceHistory[index] = true;
                getPriceHistory(ndc, index);
            } else
                if ($scope.tableRowExpanded === true) {
                    if ($scope.showIngredient[$scope.tableRowIndexExpandedCurr] == true)
                        $scope.showIngredient[$scope.tableRowIndexExpandedCurr] = false;

                    if ($scope.tableRowIndexExpandedCurr === index && $scope.ndcExpanded === ndc) {
                        $scope.tableRowExpanded = false;
                        $scope.tableRowIndexExpandedCurr = "";
                        $scope.ndcExpanded = "";
                        $scope.showPriceHistory[index] = false;
                    } else {
                        $scope.tableRowIndexExpandedPrev = $scope.tableRowIndexExpandedCurr;
                        $scope.tableRowIndexExpandedCurr = index;
                        $scope.ndcExpanded = ndc;
                        $scope.showPriceHistory[$scope.tableRowIndexExpandedPrev] = false;
                        $scope.showPriceHistory[$scope.tableRowIndexExpandedCurr] = true;
                        getPriceHistory(ndc, index);
                    }
            }
        };

        //----------------------------------
        // Reset to not display ingredients
        function resetShowIngredientFlags() {
            $scope.showIngredient = [];
            for (var i = 0; i < $scope.pharmaResult.length; i += 1) {
                $scope.showIngredient.push(false);
            }
        };
        //----------------------------------
        // Display ingredients of the ndc under cell with index
        $scope.displayIngredient = function (index, ndc) {
            if (typeof $scope.showIngredient === 'undefined') {
                resetShowIngredientFlags();
            }

            if ($scope.tableRowExpanded === false && $scope.tableRowIndexExpandedCurr === "" && $scope.ndcExpanded === "") {
                $scope.tableRowIndexExpandedPrev = "";
                $scope.tableRowExpanded = true;
                $scope.tableRowIndexExpandedCurr = index;
                $scope.ndcExpanded = ndc;
                $scope.showIngredient[index] = true;
                getIngredient(ndc, index);
            } else
                if ($scope.tableRowExpanded === true) {
                    if ($scope.showPriceHistory[$scope.tableRowIndexExpandedCurr] == true)
                        $scope.showPriceHistory[$scope.tableRowIndexExpandedCurr] = false;

                    if ($scope.tableRowIndexExpandedCurr === index && $scope.ndcExpanded === ndc) {
                        $scope.tableRowExpanded = false;
                        $scope.tableRowIndexExpandedCurr = "";
                        $scope.ndcExpanded = "";
                        $scope.showIngredient[index] = false;
                    } else {
                        $scope.tableRowIndexExpandedPrev = $scope.tableRowIndexExpandedCurr;
                        $scope.tableRowIndexExpandedCurr = index;
                        $scope.ndcExpanded = ndc;
                        $scope.showIngredient[$scope.tableRowIndexExpandedPrev] = false;
                        $scope.showIngredient[$scope.tableRowIndexExpandedCurr] = true;
                        getIngredient(ndc, index);
                    }
            }
        };

        //----------------------------------
        // Export the currently displayed tableID table data to EXCEL 
        $scope.exportToExcel = function (tableID) {
            closeChildTable();
            setTimeout(
                function () {
                    export_table_to_excel(tableID);
                }, 1000);
         };

        //----------------------------------
        // Doule clicking GPI number kicks off another search based on the selected GPI
         $scope.filterByGPI = function (gpi) {
            $('#rda_search_type_GPI').prop("checked", true);
            $scope.changeRadioSelection('GPI');
            $scope.gpi = gpi;
            $scope.ndc = '';
            $scope.product = '';
            $scope.groupCode = '';
            $scope.classCode = '';
            $scope.manufacturer = '';
            $scope.typedGroupClass = '';
            $scope.search();
         }
        //----------------------------------
        // Close any ingredient or price history child table
        function closeChildTable() {
            if ($scope.tableRowExpanded) {
                
                if ($scope.showPriceHistory[$scope.tableRowIndexExpandedCurr] == true) {
                    $scope.showPriceHistory[$scope.tableRowIndexExpandedCurr] = false;
                } 
                if ($scope.showIngredient[$scope.tableRowIndexExpandedCurr] === true)  {  
                    $scope.showIngredient[$scope.tableRowIndexExpandedCurr] = false; 
                }

                $scope.tableRowExpanded = false;
                $scope.tableRowIndexExpandedCurr = "";
                $scope.tableRowIndexExpandedPrev = "";
                $scope.ndcExpanded = "";
            }
        };

        //----------------------------------
        // Retrieve Price HIstory information for the given ndc in row#=index
        function getPriceHistory(ndc, index) {
            var params = { NDC: ndc };
            $("body").toggleClass("wait");
            DataService.Pharma_Price_History(params)
            .then(function (p1) {
                $scope.pharmaPriceHistory = p1.data;
               
                if ($scope.pharmaPriceHistory.length > 0) {
                    $scope.showPriceHistory[index] = true;
                }
            },
                function (errorP1) {
                    $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp Error loading drug price history';
                }
            )
            $("body").toggleClass("wait");
        };
        //----------------------------------
        // Retrieve ingredient information for the given ndc in row#=index
        function getIngredient(ndc, index) {
            var params = { NDC: ndc };
            $("body").toggleClass("wait");
            DataService.Pharma_Ingredient(params)
            .then(function (p1) {
                $scope.pharmaIngredient = p1.data;

                if ($scope.pharmaIngredient.length > 0) {
                    $scope.showIngredient[index] = true;
                }
            },
                function (errorP1) {
                    $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp Error loading ingredient';
                }
            )
            $("body").toggleClass("wait");
        };

        //----------------------------------
        // Toggle function when a user selects any Generic Name to display/close the list of all generic names in the current data set
        $scope.displayGenericNames = function () {
            $scope.showGenericNames = true;
        }
        //----------------------------------
        // Retrieve unique Generic Names to display on the modal when a user selects any Generic Name
        function getGenericNames() {
            var params = {
                Group_Code: $scope.groupCode,
                Class_Code: $scope.classCode,
                NDC: $scope.ndc,
                GPI: $scope.gpi,
                PRODUCT: $scope.product,
                MANUFACTURER: $scope.manufacturer,
                GroupClass_Name: $scope.typedGroupClass
            };
            $("body").toggleClass("wait");

            // Retrieve data
            DataService.Pharma_GenericNames(params)
            .then(function (p1) {
                var name_cnt = p1.data.length;
                $scope.pharmaGenericNames = p1.data.slice(0, 20);  // Only show up to 20 names as per the Ray

                if (name_cnt > 20) {
                    $scope.pharmaGenericNames.push({ generic_name: " " });
                    $scope.pharmaGenericNames.push({ generic_name: "----- PLEASE LIMIT YOUR SEARCH TO REDUCE THE LIST -----" });
                };
            },
                function (errorP1) {
                    $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp Error loading generic names';
                }
            )
            $("body").toggleClass("wait");
        };

        //-------------------------------------------
        // SEARCH button is pressed to read in the user data entry and retrieve a list of drugs satisfying the data in params
        $scope.search = function () {
            var params = {
                Group_Code: $scope.groupCode,
                Class_Code: $scope.classCode,
                NDC: $scope.ndc,
                GPI: $scope.gpi,
                PRODUCT: $scope.product,
                MANUFACTURER: $scope.manufacturer,
                GroupClass_Name: $scope.typedGroupClass
            };
          

            // manually read as ng-model doesn't seem to catch the value
            params.GroupClass_Name = $('#therapeutic-groupclass').val();

            $scope.displayMessage = '';
            $scope.errorMessage = '';
            $scope.showReport = false;
            $scope.predicate = 'name';
            $scope.reverse = false;

            if ($scope.groupCode == '' && $scope.classCode == '' && $scope.ndc == '' &&
                $scope.gpi == '' && $scope.product == '' && $scope.manufacturer == '' && $scope.typedGroupClass == '') {
                $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp At least one selection or data entry must be made.';
            }
            else
                if (isNaN($scope.ndc)) {
                    console.log($scope.ndc);
                    $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp NDC must be numeric ';
                    $('#NDC').focus();
                }
               
                    else {
                        $scope.displayMessage = '<i class="fa fa-spinner fa-spin fa-lg"></i> &nbsp Retrieving data....';
                        $("body").toggleClass("wait");
                       
                        // Retrieve data from the service
                        DataService.Pharma_Result(params)
                          .then(function (p1) {
                              // Get result set
                              $scope.pharmaResult = p1.data;  
                              if ($scope.pharmaResult.length > 0) {
                                  resetShowPriceHistoryFlags();
                                  resetShowIngredientFlags();

                                  $scope.displayMessage = '';
                                  $scope.errorMessage = '';
                                  $scope.tableRowExpanded = false;
                                  $scope.tableRowIndexExpandedCurr = "";
                                  $scope.tableRowIndexExpandedPrev = "";
                                  $scope.ndcExpanded = "";
                                  $scope.showReport = true;
                                  $scope.showGenericNames = false;

                                  // Get a list of unique Generic Names to display when a user click on any Generic Name 20160413
                                  getGenericNames();                                  
                              }
                              else {
                                  $scope.displayMessage = 'No records found';
                              }
                          },
                            function (errorP1) {
                                $scope.displayMessage = '';
                                $scope.errorMessage = '<i class="fa fa-exclamation-triangle fa-lg"></i> &nbsp Error retrieving data';
                                 
                                $("body").toggleClass("wait");
                                return;
                            }
                          );
                        $("body").toggleClass("wait");
                    }
             
        };

        // jQuery DATATABLE - works but need to add formatting of numeric, currency, & date
        // function PharmaGroupClassRetriever ($http, $q, $timeout) {
        //     var retriever = new Object();

        //     PharmaGroupClassRetriever.getdata = function (i) {
        //         var pharmaList = $q.defer();
        //         var data;

        //         //if (i && i.indexOf('T') != -1)
        //         //    movies = moreMovies;
        //         //else
        //         data = $scope.Pharma_GroupClass;

        //         $timeout(function () {
        //             pharmaList.resolve(data);
        //         }, 1000);

        //         return pharmaList.promise
        //     }
        //     return PharmaGroupClassRetriever;
        //};
        //$scope.filterPharmaGroupClass = function (typed) {
        //    console.log(typed);
        //    $scope.newPharmaGroupClassList = PharmaGroupClassRetriever.getdata(typed);

        //    $scope.newmovies.then(function (data) {
        //        $scope.pharmaManufacturers = angular.fromJson(p1.data);
        //    });
        //}

        //$.widget("custom.custom_autocomplete", $.ui.autocomplete, {
        //    _renderMenu: function (ul, items) {
        //        var self = this;
        //        var html = [];
        //        html.push("<span class='tin'>" + document.getElementById('ddlRptType').value + "</span>");
        //        //html.push("<span class='juris'>Juris</span>");
        //        html.push("<span class='name'>Name</span>");
        //        //html.push("<div style=clear:left;></div>");
        //        $("<li class='lookup lookup-header ui-state-disabled'></li>")// ui-category
        //            .data('ui-autocomplete-item', { value: '' })
        //            .append("<a>" + html.join('') + "</a>")
        //            .appendTo(ul);
        //        $.each(items, function (index, item) {
        //            self._renderItemData(ul, item);
        //        });
        //        $(ul).scrollTop(0);
        //    },
        //    _renderItem: function (ul, item) {
        //        var html = [];
        //        html.push("<a>");
        //        if (item.gr("policy_no") || item.hasOwnProperty("prospect_no")) {
        //            html.push("<span class='prospol_no'>" + (item.policy_no || item.prospect_no) + "</span>");
        //            html.push(" - ");
        //        }
        //        if (item.hasOwnProperty("insured_name")) {
        //            var insured_name = item.insured_name;
        //            if (item.hasOwnProperty("reports") && item.account_id === null) {
        //                insured_name = "Create a new account";
        //            }
        //            html.push("<span class='insured'>" + insured_name + "</span>");
        //        }
        //        if (item.hasOwnProperty("reports")) {
        //            //item is new_business
        //            html.push("<div>");
        //            item.reports.forEach(function (report) {
        //                html.push("<button class='newBusinessCustomerBtn btn btn-default btn-sm' data-step_id='" + report.step_id + "' data-report_id='" + report.report_id + "' data-account_id='" + item.account_id + "'" + (report.step_id > 1 ? " disabled='disabled' title='" + report.step_name + " coming soon.' style='pointer-events:auto;'" : "") + ">" + (report.report_id == null ? "Start" : "Edit") + " " + report.step_name + "</button>");
        //            });
        //            html.push("</div>");
        //        }
        //        html.push("</a>");
        //        var $li = $("<li class='lookup'>")
        //            .data('ui-autocomplete-item', item)
        //            .append(html.join(''))
        //            .appendTo(ul);
        //        if (item.hasOwnProperty("reports")) {
        //            $li.on('click', function (e) {
        //                if (e.target.tagName.toLowerCase() !== 'button') {
        //                    e.preventDefault();
        //                    return false;
        //                }
        //            });
        //        }
        //        return $li;
        //    }
        //});
        //function createReportDataObject(data) {
        //    $scope.pharmaResultTable = $('#pharma_reportTable').DataTable({
        //        "language": {
        //            "emptyTable": "No Pharmaceuticals found."
        //        },
        //        dom: 'Bfrtip',
        //        buttons: [ 
        //            'copyHtml5',
        //            'excelHtml5',
        //            //'csvHtml5',
        //            'pdfHtml5'
        //        ],
        //        "sExtends": "print",
        //        "paging": false,
        //        "pagingType": "full_numbers",
        //        "lengthMenu": [[50, 100, -1], [50, 100, "All"]],
        //        "order": [[1, "asc"]],
        //        "processing": true,
        //        "deferRender": true,
        //        "aoColumnDefs": [
        //            { "sClass": "date_column", "aTargets": [ 11 ] }
        //        ],
        //        "data": data,
        //        "columns": [{//"targets": 0,
        //            "title": "Old Prices",
        //            "className": 'details-control',
        //            "orderable": false,
        //            "data": null,
        //            "defaultContent": '',
        //            "visible": true,
        //            "width": "20px"
        //        }, { 
        //            "title": "NDC",
        //            "data": "ndc",
        //            "width": "60px",
        //            "visible": true
        //        }, {
        //            "title": "Name",
        //            "data": "name",
        //            "visible": true
        //        }, {
        //            "title": "GPI",
        //            "data": "gpi",
        //            "visible": true
        //        }, {
        //            "title": "Generic_Name",
        //            "data": "generic_name",
        //            "visible": true
        //        }, {
        //            "title": "Rx_OTC",
        //            "data": "rx_otc",
        //            "visible": true
        //        }, {
        //            "title": "Source",
        //            "data": "source",
        //            "visible": true
        //        }, {
        //            "title": "Manufacturer",
        //            "data": "manufacturer",
        //            "visible": true
        //        }, {
        //            "title": "Unit Size",
        //            "data": "unit_size",
        //            "visible": true
        //        }, {
        //            "title": "Pkg Qty",
        //            "data": "pkg_qty",
        //            "visible": true,
        //            "className": "right_align"
        //        }, {
        //            "title": "Measure Unit",
        //            "data": "measure_unit",
        //            "visible": true,
        //            "className": "center_align"
        //        }, {
        //            "title": "Unit Price",
        //            "data": "unit_price",
        //            "visible": true,
        //            "className": "right_align"
        //        }, {
        //            "title": "Pkg Price",
        //            "data": "pkg_price",
        //            "visible": true,
        //            "className": "right_align"
        //        }, {
        //            "title": "Price Effective",
        //            "data": "price_effective_date",
        //            "visible": true,
        //            "dateFormat": "mm-dd-yy"
        //        }, {
        //            "title": "Past Unit Price",
        //            "data": "older_unit_price",
        //            "visible": false,
        //            "className": "right_align",
        //            "type": Number
        //        }, {
        //            "title": "Past Pkg Price",
        //            "data": "older_pkg_price",
        //            "visible": false,
        //            "className": "right_align",
        //            "type": Number
        //        }, {
        //            "title": "Past price effective",
        //            "data": "older_price_effective_date",
        //            "visible": false,
        //            "dateFormat": "mm-dd-yy"
        //        }, {
        //            "title": "DEA",
        //            "data": "dea",
        //            "visible": true
        //        }, {
        //            "title": "Dosage Form",
        //            "data": "dosage_form",
        //            "visible": true
        //        }, {
        //            "title": "Therapeutic Group",
        //            "data": "therapeutic_group",
        //            "visible": true
        //        }, {
        //            "title": "Therapeutic Class",
        //            "data": "therapeutic_class",
        //            "visible": true
        //        }, {
        //            "title": "Name Type",
        //            "data": "name_type",
        //            "visible": false
        //        }
        //      ]
        //    });
        //};
    };
   
})();