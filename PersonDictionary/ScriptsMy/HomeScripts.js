
    $(document).ready(function ()
    {
        $('.CloseForm').bind('click', function () {
            $('div#logOrRegistPlace').empty();
            });
        function setHeight() {
            var H= $('.AutoHeight').css({
                height: ($(window).height()) -
                    ($('header').css('height')) -
                    ($('footer').css('height')) + 'px'
            });
        }
        setHeight(); // устанавливаем высоту окна при первой загрузке страницы
        $(window).resize(setHeight); // обновляем при изменении размеров окна
        
    });
