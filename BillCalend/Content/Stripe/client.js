// A reference to Stripe.js initialized with your real test publishable API key.
var stripe = Stripe("pk_test_51JHY9HDYEkAcurai4f9jnaPahWCYKJZ2ZWA3KBkPLY3TQAlRWw0dzLh5Q69nRLINiMvMTCRd2AwrP0Qka5drnJcG00qVbrW4WD");
//var stripe = Stripe("pk_live_51JHY9HDYEkAcuraiKgIMo2pfyqswso3PIXObY0wqkCYVSanqhAuHvOH07JzH5qSExTUo39Q6i4T35Ik5xRMhp5HL00Pn0DVhBG");

// The items the customer wants to buy
var purchase = {
    items: '001'
};

document.getElementById('ddlDiscountTerms').addEventListener('change', function (value, val) {

    var pricingValue = document.getElementById('ddlDiscountTerms').value;
    purchase.items = pricingValue;

    if (pricingValue !== '001') {

        $('#trynowbtn').hide();
        document.getElementById('payment-form').style.display = 'block';

        // Disable the button until we have Stripe set up on the page
        document.querySelector("button").disabled = true;

        fetch("/app/CreatePaymentIntent", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(purchase)
        })
            .then(function (result) {
                return result.json();
            })
            .then(function (data) {
                var elements = stripe.elements();

                var style = {
                    base: {
                        color: "#32325d",
                        fontFamily: 'Arial, sans-serif',
                        fontSmoothing: "antialiased",
                        fontSize: "16px",
                        "::placeholder": {
                            color: "#32325d"
                        }
                    },
                    invalid: {
                        fontFamily: 'Arial, sans-serif',
                        color: "#fa755a",
                        iconColor: "#fa755a"
                    }
                };

                var card = elements.create("card", { style: style });
                // Stripe injects an iframe into the DOM
                card.mount("#card-element");

                card.on("change", function (event) {
                    // Disable the Pay button if there are no card details in the Element
                    document.querySelector("button").disabled = event.empty;
                    document.querySelector("#card-error").textContent = event.error ? event.error.message : "";
                });

                var form = document.getElementById("payment-form");
                form.addEventListener("submit", function (event) {
                    event.preventDefault();
                    // Complete payment when the submit button is clicked
                    payWithCard(stripe, card, data.clientSecret);
                });
            });
    }
    else {
        $('#trynowbtn').show();

        document.getElementById('payment-form').style.display = 'none';
    }


});

document.getElementById('trynowbtn').addEventListener('click', function () {

    $.getJSON('FinalizePayment/' + '1', function (response) {

        window.location.href = 'ApiCallService';
    });

});

// Calls stripe.confirmCardPayment
// If the card requires authentication Stripe shows a pop-up modal to
// prompt the user to enter authentication details without leaving your page.
var payWithCard = function (stripe, card, clientSecret) {
    loading(true);
    stripe
        .confirmCardPayment(clientSecret, {
            payment_method: {
                card: card
            }
        })
        .then(function (result) {
            if (result.error) {
                // Show error to your customer
                showError(result.error.message);
            } else {
                // The payment succeeded!
                orderComplete(result.paymentIntent.id);
            }
        });
};

/* ------- UI helpers ------- */

// Shows a success message when the payment is complete
var orderComplete = function (paymentIntentId) {
    loading(false);
    document
        .querySelector(".result-message a")
        .setAttribute(
            "href",
            "https://dashboard.stripe.com/test/payments/" + paymentIntentId
        );
    document.querySelector(".result-message").classList.remove("hidden");
    document.querySelector("button").disabled = true;
    $.getJSON('FinalizePayment', function (response) {

        window.location.href = 'ApiCallService';
    });

};

// Show the customer the error from Stripe if their card fails to charge
var showError = function (errorMsgText) {
    loading(false);
    var errorMsg = document.querySelector("#card-error");
    errorMsg.textContent = errorMsgText;
    setTimeout(function () {
        errorMsg.textContent = "";
    }, 4000);
};

// Show a spinner on payment submission
var loading = function (isLoading) {
    if (isLoading) {
        // Disable the button and show a spinner
        document.querySelector("button").disabled = true;
        document.querySelector("#spinner").classList.remove("hidden");
        document.querySelector("#button-text").classList.add("hidden");
    } else {
        document.querySelector("button").disabled = false;
        document.querySelector("#spinner").classList.add("hidden");
        document.querySelector("#button-text").classList.remove("hidden");
    }
};
