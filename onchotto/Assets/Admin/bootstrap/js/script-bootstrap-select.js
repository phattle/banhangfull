$(document).ready(function () 
{
    // Enable Live Search.
    $('#HsCodeId').attr('data-live-search', true);

    $('.selectHsCode').selectpicker(
    {
        width: '100%',
        title: '- [Choose HsCode] -',
        style: 'btn-warning',
        size: 6
    });
});  

