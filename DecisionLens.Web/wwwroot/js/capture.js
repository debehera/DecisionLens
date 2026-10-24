(function () {
    'use strict';

    var byId = function (id) { return document.getElementById(id); };

    var textarea = byId('discussion');
    var counter = byId('counter');
    var sampleSelect = byId('sample-select');
    var draftBtn = byId('draft-btn');
    var startBlank = byId('start-blank');
    var spinner = byId('spinner');
    var slowNote = byId('slow-note');
    var errorBox = byId('draft-error');
    var reviewPanel = byId('review-panel');
    var aiNote = byId('ai-note');
    var max = parseInt(textarea.getAttribute('data-max'), 10) || 6000;
    var busy = false;
    var samples = [];

    try {
        samples = JSON.parse(byId('samples-data').textContent);
    } catch (e) {
        samples = [];
    }

    var fieldIds = {
        title: 'Input_Title',
        decision: 'Input_Decision',
        context: 'Input_Context',
        optionsConsidered: 'Input_OptionsConsidered',
        rationale: 'Input_Rationale',
        consequences: 'Input_Consequences',
        admPhase: 'Input_AdmPhase',
        domain: 'Input_Domain'
    };

    var networkMessage = 'The AI service had a problem. Try again later or create the record manually.';

    function updateCounter() {
        var length = textarea.value.length;
        counter.textContent = length.toLocaleString('en-US') + ' / ' + max.toLocaleString('en-US');
        counter.classList.toggle('dl-counter-near', length >= max * 0.9 && length <= max);
        counter.classList.toggle('dl-counter-over', length > max);
        draftBtn.disabled = busy || length > max;
    }

    function setBusy(value) {
        busy = value;
        spinner.hidden = !value;
        updateCounter();
    }

    function showError(message) {
        errorBox.textContent = message;
        errorBox.hidden = false;
    }

    function hideError() {
        errorBox.textContent = '';
        errorBox.hidden = true;
    }

    function revealReview() {
        reviewPanel.hidden = false;
        reviewPanel.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }

    function loadSample(number) {
        for (var i = 0; i < samples.length; i++) {
            if (String(samples[i].number) === String(number)) {
                textarea.value = samples[i].text;
                sampleSelect.value = String(samples[i].number);
                updateCounter();
                return;
            }
        }
    }

    function fillForm(record, note) {
        Object.keys(fieldIds).forEach(function (key) {
            var element = byId(fieldIds[key]);
            if (element && record[key] !== undefined && record[key] !== null) {
                element.value = record[key];
            }
        });

        if (note) {
            byId('ai-confidence').textContent = note.confidence;
            byId('ai-reason').textContent = note.reason;
            aiNote.hidden = false;
        } else {
            aiNote.hidden = true;
        }
    }

    async function draft() {
        hideError();
        setBusy(true);
        slowNote.hidden = true;
        var slowTimer = setTimeout(function () { slowNote.hidden = false; }, 8000);

        try {
            var token = document.querySelector('#record-form input[name="__RequestVerificationToken"]').value;
            var response = await fetch('?handler=Draft', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ text: textarea.value })
            });

            if (!response.ok) {
                throw new Error('HTTP ' + response.status);
            }

            var result = await response.json();

            if (result.ok) {
                fillForm(result.record, result.note);
                revealReview();
            } else {
                showError(result.message || networkMessage);
            }
        } catch (e) {
            showError(networkMessage);
        } finally {
            clearTimeout(slowTimer);
            slowNote.hidden = true;
            setBusy(false);
        }
    }

    // Populate the sample dropdown
    samples.forEach(function (sample) {
        var option = document.createElement('option');
        option.value = String(sample.number);
        option.textContent = sample.number + '. ' + sample.title;
        sampleSelect.appendChild(option);
    });

    textarea.addEventListener('input', updateCounter);

    sampleSelect.addEventListener('change', function () {
        if (sampleSelect.value) {
            loadSample(sampleSelect.value);
        }
    });

    draftBtn.addEventListener('click', draft);

    startBlank.addEventListener('click', function (event) {
        event.preventDefault();
        hideError();
        aiNote.hidden = true;
        revealReview();
        var title = byId('Input_Title');
        if (title) { title.focus(); }
    });

    // Support /Capture?sample=2
    var requested = new URLSearchParams(window.location.search).get('sample');
    if (requested) {
        loadSample(requested);
    }

    updateCounter();
})();