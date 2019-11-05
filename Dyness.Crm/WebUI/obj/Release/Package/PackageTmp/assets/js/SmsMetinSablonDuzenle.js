$(function () {
    $("#allfields li").draggable({
        appendTo: "body",
        helper: "clone",
        cursor: "select",
        revert: "invalid"
    });
    initDroppable($("#Model_Sablon"));
    function initDroppable($elements) {
        $elements.droppable({
            hoverClass: "textarea",
            accept: ":not(.ui-sortable-helper)",
            drop: function (event, ui) {
                var $this = $(this);

                var tempid = ui.draggable.text();
                var dropText;
                dropText = " {" + tempid + "} ";
                var droparea = document.getElementById('Model_Sablon');
                var range1 = droparea.selectionStart;
                var range2 = droparea.selectionEnd;
                var val = droparea.value;
                var str1 = val.substring(0, range1);
                var str3 = val.substring(range1, val.length);
                droparea.value = str1 + dropText + str3;
            }
        });
    }
});