function UserPrompt(title, body, theme, image) {
    $.Notify({
        caption: title,
        content: body,
        icon: "<span class='mif-" + image + "'></span>",
        type: theme
    });
}
