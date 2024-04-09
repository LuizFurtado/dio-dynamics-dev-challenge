function getCep(primaryControl) {
    var globalContext = Xrm.Utility.getGlobalContext();
    var formContext = primaryControl;

    var cep = formContext.getControl("address1_postalcode").getAttribute().getValue();

    console.log("address1_postalcode: " + cep);

    if(cep) {
        cep = cep.replace(/\D/g, '');
        var parameters = {
            "CepInput": cep
        };

        var req = new XMLHttpRequest();
        req.open("POST", globalContext.getClientUrl() + "/api/data/v9.2/dpp_CepRequest", true);
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");

        req.onreadystatechange = function() {
            if (this.readyState == 4) {
                if (this.status == 200 || this.status == 204) {
                    var result = JSON.parse(this.response);
                    const jsonCep = JSON.parse(result.CepOutput);
                    console.log(jsonCep);
                    formContext.getAttribute("address1_line1").setValue(jsonCep.logradouro);
                    formContext.getAttribute("address1_city").setValue(jsonCep.localidade);
                    formContext.getAttribute("address1_stateorprovince").setValue(jsonCep.uf);
                } else {
                    var error = JSON.parse(this.response).error;
                    Xrm.Utility.alertDialog(error.message);
                }
            }
        };

        req.send(JSON.stringify(parameters));
    }
}