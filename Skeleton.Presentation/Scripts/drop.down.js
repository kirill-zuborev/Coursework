$(document).ready(function () {
    (function ($) {
         
        function Instance(node, onSelectChenged, searchFunction) {
            var instance = this;
            var onSelectChenged = onSelectChenged;
            var node = node;
            var searchFunction = searchFunction;

            this.setOnSelectChenged = function (func) {
                onSelectChenged = func;
            }
            this.setSearchFunction = function (func) { 
                searchFunction = func;
            }

            this.deleteItems = function () {
                updateItems();
            }
            this.getSelected = getSelected;
            this.updateItems = updateItems;
            

            function getSelected() {
                return  node
                    .children('.drop-down')
                    .children('.drop-down__select')
                    .children('.drop-down__item-selected-prev')
                    .data()
                    .data;
            }

            function updateItems(params) {
                node.children('.drop-down').children('.drop-down__select').children()
                    .remove('.drop-down__item-selected-prev').parent().append("<div class = 'drop-down__item-selected-prev'></div>")
                    .children('.drop-down__item-selected-prev').append('Select');

                var nodeItemList = node.children('.drop-down').children('.drop-down__items').children('.drop-down__items-list');
                nodeItemList.empty();

                var obj = $.extend({ items: [] }, { items: params });

                for (var i = 0; i < obj.items.length; i++) {
                    nodeItemList.append("<div class='drop-down__item'><div class='drop-down__item-selected-prev'>" + obj.items[i].hidden + "</div>" +
                        "<div class='drop-down__item-list-prev'>" + obj.items[i].list + "</div></div>");
                    nodeItemList.children('.drop-down__item').last().children('.drop-down__item-selected-prev').data({'data' : obj.items[i].data,  'searchField': obj.items[i].searchField });
                }

                $('.drop-down__input', node).on('input propertychange', function () {
                    searchFunction(this);
                });
                nodeItemList.children('.drop-down__item').on('click', onClickItem);
            }

            function onClickItem() {
                var nodePreview = $(this).parents('.drop-down').children('.drop-down__select').click();
                nodePreview = nodePreview.children('.drop-down__item-selected-prev').empty();
                 
                nodePreview.append($(this).children('.drop-down__item-selected-prev').html());
               
                nodePreview.data($(this).children('.drop-down__item-selected-prev').data());
                 
                if (!(onSelectChenged == null)) { 
                    onSelectChenged(instance);
                }
            }
        }   

        $.fn.dropDown = function (params) {
            var obj = $.extend({
                items: [],
                onSelectChanged: null,
                searchFunction: search
            }, params);
            var instance = new Instance(this, obj.onSelectChanged, obj.searchFunction);
            makeMarkup(this);
            instance.updateItems(obj.items);

            $(document).on('click', function (event) {
                $('.drop-down', document)
                    .not($(event.target).parents('.drop-down'))
                    .removeClass('drop-down_opened').addClass('drop-down_closed');
            });
            return instance;
        }

        function makeMarkup(node) {
            node.append("<div class='drop-down drop-down_closed'>" +
                            "<div class='drop-down__select'>" +
                                "<div class='drop-down__item-selected-prev'></div>" +
                                "<div class='drop-down__arrow'></div>" +
                            "</div>" +
                            "<div class='drop-down__items'>" +
                                "<div style='padding:4px;'>"  +
                                    "<input class='drop-down__input' type='text'>" +
                                "</div>" +
                                "<div class='drop-down__items-list'></div>" +
                            "</div></div>");
            $('.drop-down__select', node).on('click', onClickDropDown); 
        }

        function search(node) {
            var items = $(node)
                .parents('.drop-down__items').children('.drop-down__items-list')
                .children('.drop-down__item').removeClass('drop-down__item_hidden');
            var textInput = $(node).val().toLowerCase();

            if (!(textInput == "")) {
                $.each(items, function (index, value) {
                    var selectedPreview = $(value).children('.drop-down__item-selected-prev');
                    var itemText = selectedPreview.data().data[selectedPreview.data().searchField];
                    if (!(itemText.toLowerCase().indexOf(textInput) + 1)) {
                        $(value).addClass('drop-down__item_hidden');
                    }
                })
            } 
        }

        function onClickDropDown(event) {
            var nodeDropDown = $(this).parent();
             
            if (nodeDropDown.hasClass('drop-down_closed')) {
                nodeDropDown
                    .removeClass('drop-down_closed')
                    .addClass('drop-down_opened');
            }
            else {
                nodeDropDown
                    .removeClass('drop-down_opened')
                    .addClass('drop-down_closed');
            }

            nodeDropDown.children('.drop-down__items').children('div')
                .children('.drop-down__input').val("");
            nodeDropDown
                .children('.drop-down__items').children('.drop-down__items-list')
                .children('.drop-down__item').removeClass('drop-down__item_hidden');
        }
    })(jQuery);
})