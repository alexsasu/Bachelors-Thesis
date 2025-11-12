import numpy as np
import pickle
import sys
import web_page_info_extractor


def load_xgb_model(path):
    loaded_model = pickle.load(open(path, 'rb'))
    return loaded_model


def get_web_page_info(url, model):
    features, domain_name = web_page_info_extractor.main(url)
    features = np.array(features)
    features = features.reshape(1, -1)

    prediction = model.predict(features)

    return bool(prediction), domain_name


def main():
    url = sys.argv[1]

    xgb_model_path = r"C:\Users\Alex\Desktop\Diverse\GitHub\Repos\Bachelor-Thesis\bachelor-project-ai\xgb.sav"
    model = load_xgb_model(xgb_model_path)

    prediction, domain_name = get_web_page_info(url, model)

    if not prediction:
        print("Safe")
    elif prediction:
        print("Suspicious")

    print(domain_name)


if __name__ == "__main__":
    main()
