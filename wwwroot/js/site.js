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
