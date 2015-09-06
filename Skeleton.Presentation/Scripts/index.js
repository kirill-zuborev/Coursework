$(document).ready(function () {
      
    $.ajax({
        type: "GET",
        url: 'Home/GetAllFoldersByUserId',
        dataType: "json",
        data: {userId: 1},
        success: function (data) {
            var nodeFoldersList = $('.folders__list'); 
            $.each(data, function (index, value) {
                addFolderToFoldersList(value, nodeFoldersList);
            });
            nodeFoldersList.children().first().children('.folder__name').click();
        }
    });

    $('#dp1').datetimepicker({
        locale: 'en',
        widgetPositioning: {
            vertical: 'bottom',
            horizontal: 'left'
        }
    }); 

    function addSubgoal() {
        var subgoalName = $.trim($('.subgoals__edit textarea').val());
        if (subgoalName != '') {
            $.ajax({
                type: "GET",
                url: 'Home/AddSubgoal',
                data: { goalId: $('.goal_selected').data().Id, name: subgoalName },
                success: function (data) {
                    addSubgoalToSubgoalsList($('.subgoals__list'), { Name: subgoalName, Id: data });
                    $('.subgoals__list').children().last().children('.subgoal__delete').on('click', deleteSubgoal);
                    $('.subgoals__list').children().children('.subgoal__checkbox').children('.subgoal__input-checkbox').on('change', changeIsDoneInSubgoal);
                    $('.subgoals__edit textarea').val('');
                }
            });
        }
    }
    function addFolder() {
        var folderName = $.trim($('.popup-add-folder__input').val());
        if (folderName != '') {
            $.ajax({
                type: "GET",
                url: 'Home/AddFolder',
                dataType: "json",
                data: { name: folderNamed, userId: 1 },
                success: function (data) {
                    nodeFolderList = $('.folders__list');
                    addFolderToFoldersList({ Id: data, Name: folderName }, nodeFolderList);
                    $('.popup-add-folder').addClass('hidden');
                    $('.popup-add-folder__input').val('');
                }
            });
        }
    }
    function addGoal(event) {
        if (event.keyCode == 13) {
            var goalName = $.trim($('.goals__input-add').val());
            if (goalName != '') {
                var folderId = $('.folder_selected').data().Id;
                $.ajax({
                    type: "GET",
                    url: 'Home/AddGoal',
                    data: { name: goalName, folderId: folderId },
                    dataType: "json",
                    success: function (data) {
                        var nodeGoalList = $('.goals__list');
                        addGoalToGoalsList(
                            {
                                Id: data,
                                Name: goalName,
                                IsDone: false,
                                FolderId: folderId
                            }, nodeGoalList);
                        nodeGoalList.children().children('.goal__name').on('click', clickGoal);
                        nodeGoalList.children().children('.goal__checkbox').children('.goal__input-checkbox').on('change', changeIsDoneInGoal);
                        $('.goals__input-add').val('');
                    }
                });

            }
        }
    }

    function selectFolder(ev) {
        $('.goals__add').removeClass('hidden');
        $('.goals').addClass('goals_full')
        $('.datail').addClass('datail_hidden');
        $('.folders__list').children().removeClass('folder_selected');
        $(ev.target).parent().addClass('folder_selected');
        var folderId = $(ev.target).parent().data().Id;
        $.ajax({
            type: "GET",
            url: 'Home/GetGoalsByFolderId',
            dataType: "json",
            data: { folderId: folderId },
            success: function (data) {

                var nodeGoalList = $('.goals__list');
                nodeGoalList.empty();
                $.each(data, function (index, value) {
                    addGoalToGoalsList(value, nodeGoalList);
                });


                nodeGoalList.children().children('.goal__name').on('click', clickGoal);
                nodeGoalList.children().children('.goal__checkbox').children('.goal__input-checkbox').on('change', changeIsDoneInGoal);
            }
        });
    }
    function clickGoal() {
        $('.goals__list').children().removeClass('goal_selected');
        $(this).parent().addClass('goal_selected');

        $('.subgoal__edit-box').val('');

        var data = $(this).parent().data();

        if (data.IsStarred) {
            $('.head__mark').addClass('head__mark_starred');
        }
        else {
            $('.head__mark').removeClass('head__mark_starred');
        }

        if (data.IsDone) {
            $('.head__checkbox').children().prop('checked',true);
        }
        else {
            $('.head__checkbox').children().prop('checked', false);
        }

        $('.goals').removeClass('goals_full')
        $('.datail').removeClass('datail_hidden');

        $('#dp1').val('');
        if (data.DueDate != null) {
            $('#dp1').data("DateTimePicker").date(data.DueDate);
        }

        $('.head__name').empty();
        $('.head__name').append(data.Name);

        $('.description__edit-box').val(data.Description);

        $.getJSON('Home/GetSubgoalsByGoalId', { goalId: data.Id }, function (data) {
            $('.subgoals__list').empty();
            var nodeSubgoalList = $('.subgoals__list');
            $.each(data, function (index, value) {
                addSubgoalToSubgoalsList(nodeSubgoalList, value);
            });
            nodeSubgoalList.children().children('.subgoal__delete').on('click', deleteSubgoal);
            nodeSubgoalList.children().children('.subgoal__checkbox').children('.subgoal__input-checkbox').on('change', changeIsDoneInSubgoal);
        })
    }

    function changeIsDoneInGoal(event) {
        var checkbox = $(event.target);
        var id = checkbox.hasClass('goal__input-checkbox') ?
            checkbox.parent().parent().data().Id : $('.goal_selected').data().Id;
        
        $.ajax({
            type: "GET",
            url: 'Home/ChangeIsDoneInGoal',
            data: { goalId: id },
            success: function () {
                var checkbox = $(event.target);
                var value = checkbox.prop('checked');
                if (checkbox.hasClass('goal__input-checkbox')) {
                    checkbox.prop('checked', value);
                    if (checkbox.parent().parent().hasClass('goal_selected')) {
                        $('.head__input-checkbox').prop('checked', value);
                    }
                    checkbox.parent().parent().data().IsDone = value; 
                    if (value) {
                        checkbox.parent().parent().addClass('goal_done');
                    }
                    else {
                        checkbox.parent().parent().removeClass('goal_done');
                    }
                }
                else {
                    $('.goal_selected').children('.goal__checkbox').children('.goal__input-checkbox').prop('checked', value);
                    checkbox.prop('checked', value);
                    var data = $('.goal_selected').data().IsDone = value; 
                    if (value) {
                        $('.goal_selected').addClass('goal_done');
                    }
                    else {
                        $('.goal_selected').removeClass('goal_done');
                    }
                }
            }
        });
    }
    function changeIsDoneInSubgoal(event) {
        $.ajax({
            type: "GET",
            url: 'Home/ChangeIsDoneInSubgoal',
            data: { subgoalId: $(event.target).parent().parent().data().id },
            success: function () {
                if ($(event.target).prop('checked')) {
                    $(event.target).parent().parent().addClass('subgoal_done')
                }
                else {
                    $(event.target).parent().parent().removeClass('subgoal_done')
                }

            }
        });
    }

    function addGoalToGoalsList(value, nodeGoalList) {
        if (value.DueDate != null) {
            var date = new Date(parseInt(value.DueDate.substr(6)));
            var dat = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
        }
        else {
            var dat = '';
        }
        nodeGoalList.append(
            "<li class='goal'>" +
                "<div class='goal__checkbox'>" +
                    "<input type='checkbox' class='goal__input-checkbox'  />" +
                "</div>" +
                "<div class='goal__name'>" +
                    value.Name +
                "</div>" +
                "<div class='goal__mark'>" +
                    "&#10032;" +
                "</div> " +
                "<div class='goal__date'>" +
                    dat +
                "</div> " +

            "</li>"
            );
        nodeGoalList.children('.goal__checkbox').children('goal__input-checkbox').prop('checked', value.IsDone);
        $('.goal__mark').last().on('click', changeIsStarredInGoal);
        if (value.IsDone) {
            nodeGoalList.children().last().addClass('goal_done');
        }
        if (value.IsStarred) {
            nodeGoalList.children().last().children('.goal__mark').addClass('goal__mark_starred');
        }
        value.DueDate = date;
        nodeGoalList.children().last().data(value);
    }
    function addSubgoalToSubgoalsList(nodeSubgoalList, value) {
        var chec = value.IsDone ? 'checked' : '';
        nodeSubgoalList.append(
                    "<li class='subgoal'>" +
                        "<div class='subgoal__checkbox'>" +
                            "<input type='checkbox' class='subgoal__input-checkbox' " + chec + " />" +
                        "</div>" +
                        "<div class='subgoal__name'>" +
                            value.Name +
                        "</div>" +
                        "<div class='subgoal__delete'>" +
                            "&#10006;" +
                        "</div>" +
                    "</li>"
                    );
        nodeSubgoalList.children().last().data({ id: value.Id, name: value.Name });
        if (value.IsDone) {
            nodeSubgoalList.children().last().addClass('subgoal_done');
        }
    }
    function addFolderToFoldersList(data, nodeFoldersList) {
        nodeFoldersList.append(
            "<li class='folder'>" +
                "<div class='folder__name'>" +
                    data.Name +
                "</div>" +
                "<div class='folder__delete'>" +
                    "&#10006;" +
                "</div>" +
            "</li>"
            );
        nodeFoldersList.children().last().data(data);
        nodeFoldersList.children().last().children('.folder__name').on('click', selectFolder);
        nodeFoldersList.children().last().children('.folder__delete').on('click', deleteFolder);
    }


    function deleteGoal() {
        goalId = $('.goal_selected').data().Id;
        $.ajax({
            type: "GET",
            url: 'Home/DeleteGoal',
            data: { goalId: goalId },
            success: function () {
                $(".goal_selected").remove();
                $('.goals').addClass('goals_full')
                $('.datail').addClass('datail_hidden');
            }
        });
    }
    function deleteSubgoal() {
        var nodeSubgoal = $(this).parent();
        var subgoalId = nodeSubgoal.data().id;

        $.ajax({
            type: "GET",
            url: 'Home/DeleteSubgoal',
            data: { subgoalId: subgoalId },
            success: function () {
                nodeSubgoal.remove();
            }
        });
    }
    function deleteFolder(ev) {
        var nodeFolder = $(this).parent();
        var folderId = nodeFolder.data().Id;

        $.ajax({
            type: "GET",
            url: 'Home/DeleteFolder',
            data: { folderId: folderId },
            success: function () {
                nodeFolder.remove();
                $('.goals__list').empty();
                $('.folders__list').children().first().children('.folder__name').click();
            }
        });
    }

    function searchGoalsByName(event) {
        if (event.keyCode == 13) {
            var substring = $.trim($('.folders__input-search').val());
            if (substring != '') {
                $.ajax({
                    type: "GET",
                    url: 'Home/SearchGoalsByName',
                    data: { substring: substring },
                    dataType: "json",
                    success: function (data) {

                        var nodeGoalList = $('.goals__list');
                        nodeGoalList.empty();
                        $.each(data, function (index, value) {
                            addGoalToGoalsList(value, nodeGoalList);
                        });
                        nodeGoalList.children().children('.goal__name').on('click', clickGoal);
                        nodeGoalList.children().children('.goal__checkbox').children('.goal__input-checkbox').on('change', changeIsDoneInGoal);

                        $('.goals__add').addClass('hidden');
                        $('.folders__list').children().removeClass('folder_selected');
                    }
                });
            }
        }
    }

    function changeDescriptionInGoal() {
        var description = $.trim($('.description__edit-box').val());
        $.ajax({
            type: "GET",
            url: 'Home/ChangeDescriptionInGoal',
            data: { goalId: $('.goal_selected').data().Id, description: description },
            success: function () {
                $('.goal_selected').data().Description = description;
            }
        });
    }
    function changeIsStarredInGoal(event) {
        var star = $(event.target);
        var id = star.hasClass('goal__mark') ? star.parent().data().Id : $('.goal_selected').data().Id;

        $.ajax({
            type: "GET",
            url: 'Home/ChangeIsStarredInGoal',
            data: { goalId: id },
            success: function () {
                var star = $(event.target);
                if (star.hasClass('goal__mark')) {
                    if (star.hasClass('goal__mark_starred')) {
                        star.removeClass('goal__mark_starred');
                        star.parent().data().IsStarred = false;
                    }
                    else {
                        star.addClass('goal__mark_starred');
                        star.parent().data().IsStarred = true;
                    }
                    if (star.parent().hasClass('goal_selected')) {
                        $('.head__mark').toggleClass('head__mark_starred');
                    }
                }
                else {
                    if (star.hasClass('head__mark_starred')) {
                        $('.goal_selected').children('.goal__mark').removeClass('goal__mark_starred');
                        star.removeClass('head__mark_starred');
                        $('.goal_selected').data().IsStarred = false;
                    }
                    else {
                        $('.goal_selected').children('.goal__mark').addClass('goal__mark_starred');
                        star.addClass('head__mark_starred');
                        $('.goal_selected').data().IsStarred = true;

                    }
                }
            }
        });
    }
    function changeDateInGoal(ev) {
        var goalId = $('.goal_selected').data().Id;

        $.ajax({
            type: "GET",
            url: 'Home/ChangeDateInGoal',
            data: { goalId: goalId, date: new Date($('#dp1').val()).toUTCString() },
            success: function () {

                if ($('#dp1').val() != null) {
                    var date = new Date($('#dp1').val());
                    var dat = date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear();
                    $('.goal_selected').children('.goal__date').empty();
                    $('.goal_selected').children('.goal__date').append(dat);
                    $('.goal_selected').data().DueDate = date;
                }
            }
        });
    }


    $('#dp1').on('dp.change', changeDateInGoal);
    $('.subgoals__button-add').on('click', addSubgoal);
    $('.folsders__add-button').on('click', function () {
        $('.popup-add-folder').removeClass('hidden');
    });
    $('.popup-add-folder__close-button').on('click', function () {
        $('.popup-add-folder').addClass('hidden');
    });
    $('.popup-add-folder__add-button').on('click', addFolder);
    $('.head__input-checkbox').on('change', changeIsDoneInGoal);
    $('.goals__input-add').keyup(addGoal);
    $('.toolbox__delete').on('click', deleteGoal);
    $('.folders__input-search').keyup(searchGoalsByName);
    $('.description__button-add').on('click', changeDescriptionInGoal);
    $('.head__mark').on('click', changeIsStarredInGoal);


}) 






  


