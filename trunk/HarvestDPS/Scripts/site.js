(function ($) {
    $(document).ready(function () {


        // time entry - project/task dropdownlist change
        $(".PartialContent").on("change", "select[name='projectId']", function () {
            // empty tasks from dropdown
            var $taskDdl = $("select[name='taskid']");
            $taskDdl.empty();

            var projectId = $(this).val();
            $.get("/api/task/" + projectId, null, function (result) {
                // split task objects into billable/nonbillable by bool prop
                var billable = "";
                var nonbillable = "";
                $(result).each(function (i, e) {
                    if (e.Billable) {
                        billable += String.format("<option value='{0}'>{1}</option>", e.Id, e.Name)
                    }
                    else {
                        //nonbillable.push(e);
                        nonbillable += String.format("<option value='{0}'>{1}</option>", e.Id, e.Name)
                    }
                });

                // insert tasks into task dropdown
                $taskDdl.append("<optgroup label='Billable'>" + billable + "</optgroup>");
                $taskDdl.append("<optgroup label='Non-Billable'>" + nonbillable + "</optgroup>");

            }).error(function (result) { console.log(result); });
        });

        ///format time entry - hours textbox
        $(".entry-duration").on("blur", function () {
            var $self = $(this);
            $self.val(($self.val() * 1).toFixed(2));
        });

        $(".js-edit-entry").on("click", function () {
            var $self = $(this);
            var entry = $self.data().dayentry;
            Bind($(".edit-entry-form"), entry);
        });

        $(".PartialContent").on("click", "form .js-save", function () {
            var $self = $(this);
            var form = $self.closest("form");
            var data = form.serialize();
            data += "&project=" + $(form).find("select[name='projectId'] option:selected").text();
            data += "&task=" + $(form).find("select[name='taskid'] option:selected").text();

            $.post(form.attr("action"), data, function () {
                location.reload();
            });
        });

        // reporting static container init load
        $('.collapse').on("show.bs.collapse", function () {
            loadPartialContent(this);
        });

        // reporting static container
        $(".loadTimeEntries").on("click", ".button", function (event) {
            var $self = $(this);
            var data = $self.data();
            var target = $("#" + data.target);
            if (data.url) {
                target.data().url = data.url;
                loadPartialContent(target);
            }
        });


        $(".PartialContent").each(function () {
            loadPartialContent(this);
        });

        $("[id^=project_content], [id^=client_content]").on('hide.bs.collapse', function (event) {
            var button = $("[data-target='#" + event.target.id + "']");
            button.html('<span class="glyphicon glyphicon-collapse-down"></span>');
        });

        $("[id^=project_content], [id^=client_content]").on('show.bs.collapse', function (event) {
            var button = $("[data-target='#" + event.target.id + "']");
            button.html('<span class="glyphicon glyphicon-collapse-up"></span>');
        });
    });

    String.format = function () {
        var s = arguments[0];
        for (var i = 0; i < arguments.length - 1; i++) {
            var reg = new RegExp("\\{" + i + "\\}", "gm");
            s = s.replace(reg, arguments[i + 1]);
        }
        return s;
    }

    ///Bind an object to a form with controls where Name attr matches obj property does not ignore case
    function Bind(form, json) {

        $.each(json, function (key, value) {
            form.find("[name='" + key + "']").val(value);
        });
    }

    // Groups array into an array of objects containing an array of objects i.e. GroupBy("a", [{a:1, b:"asdf"}, {a:1, b:"qwerty"}, {a:2, b:"qwerty"}]) => [{key:1, values:[{a:1, b:asdf}, {a:1, b:qwerty}]}...]
    function GroupBy(property, array) {
        var dupes = {};
        var singles = [];

        $.each(array, function (i, e) {
            var value;
            $.each(e, function (k, v) {
                if (property == k) {
                    value = v;
                    return;
                }
            });

            if (!dupes[value]) {
                dupes[value] = true;
                singles.push({ key: value, values: [e] });
            } else {
                singles.filter(function (i) { return i.key == value; })[0].values.push(e);
            }
        });

        return singles;
    }

    // returns an array of distinct values of a given property within a given array
    function Distinct(property, array) {
        var dupes = {};
        var singles = [];
        var result = "";

        $.each(array, function (i, e) {
            var value;
            $.each(e, function (k, v) {
                if (property == k) {
                    value = v;
                    return;
                }
            });

            if (!dupes[value]) {
                dupes[value] = true;
                singles.push(value);
            }
        });

        return singles;
    }

    function loadPartialContent(item) {
        var url = $(item).data("url");
        if (url && url.length > 0) {
            $(item).load(url, function () {
                if ($(".day-view-summary .empty-view").length > 0) {
                    // set the empty quote
                    //$.get("http://quotes.rest/qod.json?category=funny", null, function (result) {
                    //    $(".day-view-summary .empty-view").html(String.format("\"{0}\" - {1}", result.contents.quotes[0].quote, result.contents.quotes[0].author));
                    //});

                    $.ajax({
                        url: "https://andruxnet-random-famous-quotes.p.mashape.com",
                        data: null,
                        type: "GET",
                        beforeSend: function (xhr) { xhr.setRequestHeader('X-Mashape-Authorization', 'ZDRLNucPDUmshnv84BUjqJQcW3ewp1jleEBjsn4t5qLdVhvu5C'); },
                        success: function (result) {
                            var obj = JSON.parse(result);
                            $(".day-view-summary .empty-view").html(String.format("\"{0}\" - {1}", obj.quote, obj.author));
                            //"quote":"I have never killed anyone, but I have read some obituary notices with great satisfaction.","author":"Clarence Darrow","category":"Famous"}
                        }
                    });
                }
            });
        }
    }
})(jQuery);
