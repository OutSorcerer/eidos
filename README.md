# eidos.ml #

## About

eidos.ml is a C# / .NET Standard library for productive development of machine learning pipelines that is based on [MathNet.Numerics](https://github.com/mathnet/mathnet-numerics).

It is inspired by [scikit-learn](https://github.com/scikit-learn/scikit-learn) but uses advantages of C# syntax and .NET runtime.

The name *eidos* comes from the Plato's [theory of Forms](https://en.wikipedia.org/wiki/Theory_of_forms), where it means non-physical (but substantial) forms (or ideas) representing the most accurate reality. Since machine learning models should simulate the reality as accurately as possible an *eidos* would be an ideal model.

## Status

Currently library is an early development stage and suggested use-case is participation in competitions like [Kaggle](https://www.kaggle.com/) and learning and experimenting with ML algorithms.

There **will** be breaking changes. But after the release of 1.0 it will be stable and suitable for production for small-scale problems (that fit to a single machine's memory).

## News

* 03.09.2017 Version 0.1 alpha is published with support of basic abstractions, Linear Regression and Ridge Regression.

## Why invent the wheel?

There are other great ML libraries for other languages and for C# too. Why create yet another?

Not only for fun. See the corresponding [blog post](https://outsorcerer.github.io/class-struggle/Why-eidos-ml/).

## Plans for the nearest future

See GitLab issues with tag *upcoming* and the road-map below.

## Road-map

| version | time frame     | features    
|---------|----------------|-------------------------------------------
|     0.1 | September 2017 | other methods for LinearRegression, RidgeRegression, documentation and tests
|     0.2 |   October 2017 | LogisticRegression, common transformations and metrics
|     0.3 |  November 2017 | TBD: most frequently used ML algorithms
|     ... |            ... | ...
|     1.0 |           2018 | stable architecture and backward compatibility, wide range of reliable and fast algorithms