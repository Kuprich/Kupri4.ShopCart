$(() => {

    $("a.confirmDeletion").click(() => {
        if (!confirm("Confirm deletion?")) return false;
    });

    if ($("div.notification").length) {

        setTimeout(() => {
            $("div.notification").fadeOut(300, () => { $("div.notificationContainer").remove(); });
        }, 2000);
    }
});

function readUrl(input) {
    console.log("readUrl");
    if (input.files && input.files[0]) {
        let reader = new FileReader();
        console.log("readUrl files");
        reader.onload = function (e) {
            $("img#imgPreview").attr("src", e.target.result).width(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}
