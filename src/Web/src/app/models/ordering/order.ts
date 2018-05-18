import { types, getRoot, getEnv, flow } from 'mobx-state-tree';
import * as validate from 'validate.js';
import uuid from 'uuid/v4';
import Debug from 'debug';

export interface OrderType {
  id: string;
  status: string;
  statusDescription: string;

  userName: string;
  buyerName: string;

  addressId: string;
  address: string;
  cityState: string;
  zipCode: string;
  country: string;

  paymentMethodId: string;
  paymentMethod: string;

  totalItems: number;
  totalQuantity: number;

  subTotal: number;

  additionalFees: number;
  additionalTaxes: number;

  total: number;

  paid: boolean;
}
export const OrderModel = types
  .model('Ordering_Order', {
    id: types.identifier(types.string),
    status: types.string,
    statusDescription: types.string,

    userName: types.string,
    buyerName: types.string,

    addressId: types.string,
    address: types.string,
    cityState: types.string,
    zipCode: types.string,
    country: types.string,
    paymentMethodId: types.string,
    paymentMethod: types.string,
    totalItems: types.number,
    totalQuantity: types.number,

    subTotal: types.number,
    additionalFees: types.number,
    additionalTaxes: types.number,
    total: types.number,
    paid: types.boolean
  });

export interface OrderIndexType {
  id: string;
  status: string;
  statusDescription: string;

  userName: string;
  buyerName: string;

  addressId: string;
  address: string;
  cityState: string;
  zipCode: string;
  country: string;

  paymentMethodId: string;
  paymentMethod: string;

  totalItems: number;
  totalQuantity: number;

  subTotal: number;

  additional: number;

  total: number;

  paid: boolean;
}
export const OrderIndexModel = types
  .model('Ordering_OrderIndex', {
    id: types.identifier(types.string),
    status: types.string,
    statusDescription: types.string,

    userName: types.string,
    buyerName: types.string,

    addressId: types.string,
    address: types.string,
    cityState: types.string,
    zipCode: types.string,
    paymentMethodId: types.string,
    paymentMethod: types.string,
    totalItems: types.number,
    totalQuantity: types.number,

    subTotal: types.number,
    additional: types.number,
    total: types.number,
    paid: types.boolean
  });