using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPShop : MonoBehaviour {
    string gold1PurchaseId = "com.DevMau.TimeDasher.100Gold";
    string gold2PurchaseId = "com.DevMau.TimeDasher.300Gold";
    string gold3PurchaseId = "com.DevMau.TimeDasher.500Gold";
    string gold4PurchaseId = "com.DevMau.TimeDasher.1000Gold";

    public GameObject restorePurchaseBtn;

    private void Awake() {
        DisableRestorePurchaseBtn();
    }

    public void OnPurchaseComplete(Product product) {
        if (product.definition.id == gold1PurchaseId) {
            BankManager.instance.AddMoneyToBank(100);
            AudioManager.instance.Play("TapCollect");
        }
        else if (product.definition.id == gold2PurchaseId) {
            BankManager.instance.AddMoneyToBank(300);
            AudioManager.instance.Play("TapCollect");
        }
        else if (product.definition.id == gold3PurchaseId) {
            BankManager.instance.AddMoneyToBank(500);
            AudioManager.instance.Play("TapCollect");
        }
        else if (product.definition.id == gold4PurchaseId) {
            BankManager.instance.AddMoneyToBank(1000);
            AudioManager.instance.Play("TapCollect");
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason) {
        Debug.Log("Purchase of " + product.definition.id + " failed due to " + reason);
    }

    private void DisableRestorePurchaseBtn() {
        if (Application.platform != RuntimePlatform.IPhonePlayer) {
            if (restorePurchaseBtn != null)
                restorePurchaseBtn.SetActive(false);
        }

    }
}
